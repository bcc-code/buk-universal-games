terraform {
  required_version = ">= 1.0"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.3.0"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "0.4.0"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.12.0"
    }
  }
  experiments = [module_variable_optional_attrs]

  backend "azurerm" {
    resource_group_name  = "BCC-Platform"
    storage_account_name = "bccplatformtfstate"
    container_name       = "tfstate"
    key                  = "buk-universal-games.terraform.tfstate"
  }
}

provider "azurerm" {
  features {}
}

provider "azapi" {
}

provider "postgresql" {
  superuser = false
  host      = data.azurerm_postgresql_flexible_server.postgresql_server.fqdn
  username  = "psqladmin"
  password  = data.azurerm_key_vault_secret.postgresql_admin_password.value
}

locals {
    location        = "WestEurope"
    resource_group  = "buk-universal-games-${terraform.workspace}"
    resource_prefix = "buk-universal-games-${terraform.workspace}"
    platform_resource_prefix = "bcc-platform-${terraform.workspace}"
    platform_resource_group  = "bcc-platform-${terraform.workspace}"
    tags            = {}
}

# Get Container Registry
data "azurerm_container_registry" "acr" {
  name                = "bccplatform"
  resource_group_name = "BCC-Platform"
}

# Get Key Vault for postgresql server
data "azurerm_key_vault" "keyvault" {
  name                = lower(replace("${local.platform_resource_prefix}-psql","-",""))
  resource_group_name = local.platform_resource_group
}

# Get Admin password for postgresql server
data "azurerm_key_vault_secret" "postgresql_admin_password" {
  name         = "postgreql-admin-password"
  key_vault_id = data.azurerm_key_vault.keyvault.id
}

# Get Postgresql Server
data "azurerm_postgresql_flexible_server" "postgresql_server" {
  name                   = "${local.platform_resource_prefix}-postgresql"
  resource_group_name    = local.platform_resource_group
}

# Get platform resource group
data "azurerm_resource_group" "platform_rg" {
  name = local.platform_resource_group
}


# Create Resource Group
resource "azurerm_resource_group" "rg" {
  name     = local.resource_group
  location = local.location
  tags     = local.tags
}

# Open DB firewall
data "http" "myip" {
  url = "http://ipv4.icanhazip.com"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "terraform_deploy_ip" {
  name             = "terraform_deploy_ip"
  server_id        = data.azurerm_postgresql_flexible_server.postgresql_server.id
  start_ip_address = "${chomp(data.http.myip.body)}"
  end_ip_address   = "${chomp(data.http.myip.body)}"
}

# Create Database
# NB! This will only work if server has a public IP and the client execuring terrafrom has been whitelisted in the server's firewall
# Perhaps whitelisting the IP address of the current client can be automated...
module "postgresql_db" {
  source             = "../../modules/postgresql_db"
  db_name            = local.resource_prefix
  server_resource_id = data.azurerm_postgresql_flexible_server.postgresql_server.id
  depends_on = [
    azurerm_postgresql_flexible_server_firewall_rule.terraform_deploy_ip
  ]
}

# Store db user password
resource "azurerm_key_vault_secret" "postgreql_user_password" {
  name         = "${local.resource_prefix}-db-user-password"
  value        = module.postgresql_db.db_user_password
  key_vault_id = data.azurerm_key_vault.keyvault.id
}

# Analytics Workspace
module "log_analytics_workspace" {
  source                           = "../../modules/log_analytics"
  name                             = "${local.resource_prefix}-logs"
  location                         = local.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags

}

# Application Insights
module "application_insights" {
  source                           = "../../modules/application_insights"
  name                             = "${local.resource_prefix}-env-insights"
  location                         = local.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags
  application_type                 = "web"
  workspace_id                     = module.log_analytics_workspace.id
}

# VLAN for Container Environment
module "container_apps_vlan" {
  source                           = "../../modules/container_apps_vlan"
  name                             = "${local.resource_prefix}-vlan"
  location                         = local.location
  resource_group_name              = azurerm_resource_group.rg.name
  tags                             = local.tags

  depends_on = [
    azurerm_resource_group.rg
  ]
}


# # Database
# resource "azurerm_postgresql_flexible_server_database" "database" {
#   name      = "${local.resource_prefix}"
#   server_id = data.azurerm_postgresql_flexible_server.postgresql_server.id
#   collation = "en_US.utf8"
#   charset   = "utf8"
# }

# Redis
resource "azurerm_redis_cache" "redis_cache" {
  name                = "${local.resource_prefix}-redis"
  location            = local.location
  resource_group_name = local.resource_group
  capacity            = 2
  family              = "C"
  sku_name            = "Basic"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"
  public_network_access_enabled = false

  redis_configuration {
  }
}

# Private DNS zone

data "azurerm_private_dns_zone" "dns_zone" {
  name                = "privatelink.redis.cache.windows.net"
  resource_group_name = local.resource_group
}

# Redis private endpoint
resource "azurerm_private_endpoint" "redis_endpoint" {
  name                = "${local.resource_prefix}-redis-endpoint"
  location            = local.location
  resource_group_name = local.resource_group
  subnet_id           = module.container_apps_vlan.subnet_id

  private_service_connection {
    name                              = "${local.resource_prefix}-redis-endpoint-connection"
    private_connection_resource_id    =  azurerm_redis_cache.redis_cache.id
    is_manual_connection              = false
    subresource_names                 = ["redisCache"]
  }

  private_dns_zone_group {
    name = "default"
    private_dns_zone_ids = [
      data.azurerm_private_dns_zone.dns_zone.id
    ]
  }
}

# Create Azure Application for Github Deployment


# Container Environment
module "container_apps_env"  {
  source                           = "../../modules/container_apps_env"
  managed_environment_name         = "${local.resource_prefix}-env"
  location                         = local.location
  resource_group_id                = azurerm_resource_group.rg.id
  tags                             = local.tags
  instrumentation_key              = module.application_insights.instrumentation_key
  workspace_id                     = module.log_analytics_workspace.workspace_id
  primary_shared_key               = module.log_analytics_workspace.primary_shared_key
  vlan_subnet_id                   = module.container_apps_vlan.subnet_id
}

# API Container App
module "api_container_app" {
  source                           = "../../modules/container_apps"
  managed_environment_id           = module.container_apps_env.id
  location                         = local.location
  resource_group_id                = azurerm_resource_group.rg.id
  tags                             = local.tags
  container_app                   = {
    name              = "${local.resource_prefix}-api"
    configuration      = {
      ingress          = {
        external       = true
        targetPort     = 5125
      }
      dapr             = {
        enabled        = true
        appId          = "${local.resource_prefix}-api"
        appProtocol    = "http"
        appPort        = 5125
      }
      secrets          = [
          {
            name    = "postgresql-password"
            value   =  module.postgresql_db.db_user_password
          },
          {
            name    = "redis-connection-string"
            value   =  azurerm_redis_cache.redis_cache.primary_connection_string
          },
          {
            name    = "application-insights-connection-string"
            value   =  module.application_insights.connection_string
          }
        ]
    }
    template          = {
      containers      = [{
        image         = "bccplatform.azurecr.io/buk-universal-games-dev-api:latest" #"hello-world:latest"
        name          = "universal-games-api"
        env           = [{
            name        = "APP_PORT"
            value       = 5125
          },
          {
            name        = "POSTGRES_HOST"
            value       = data.azurerm_postgresql_flexible_server.postgresql_server.fqdn
          },
          {
            name        = "APPLICATIONINSIGHTS_CONNECTION_STRING"
            secretRef   = "application-insights-connection-string"
          },
          {
            name        = "APPLICATIONINSIGHTS__CONNECTIONSTRING"
            secretRef   = "application-insights-connection-string"
          },
          {
            name        = "POSTGRES_PORT"
            value       = 5432
          },
          {
            name        = "POSTGRES_DB"
            value       = module.postgresql_db.db_name
          },
          {
            name        = "POSTGRES_USER"
            value       = module.postgresql_db.db_user_username
          },
          {
            name        = "POSTGRES_PASSWORD"
            secretRef   = "postgresql-password"
          },
          {
            name        = "REDIS_CONNECTION_STRING"
            secretRef   = "redis-connection-string"
          },
          {
            name        = "ENVIRONMENT_NAME"
            value   = terraform.workspace
          }
        ]
        resources     = {
          cpu         = 0.5
          memory      = "1Gi"
        }
      }]
      scale           = {
        minReplicas   = 0
        maxReplicas   = 10
      }
    }
  }
}

# Generate password
resource "random_password" "directus_admin_pw" {
  length           = 32
  special          = true
  override_special = "!#*()-_=+[]:?"
}

resource "random_uuid" "directus_key" {
}
resource "random_uuid" "directus_secret" {
}

# Directus Container App
module "directus_container_app" {
  source                           = "../../modules/container_apps"
  managed_environment_id           = module.container_apps_env.id
  location                         = local.location
  resource_group_id                = azurerm_resource_group.rg.id
  tags                             = local.tags
  container_app                   = {
    name              = "${local.resource_prefix}-directus"
    configuration      = {
      ingress          = {
        external       = true
        targetPort     = 8055
      }
      dapr             = {
        enabled        = true
        appId          = "${local.resource_prefix}-directus"
        appProtocol    = "http"
        appPort        = 8055
      }
      secrets          = [
        {
          name    = "postgresql-password"
          value   =  module.postgresql_db.db_user_password
        },
        {
          name    = "redis-connection-string"
          value   =  "rediss://:${azurerm_redis_cache.redis_cache.primary_access_key}@${azurerm_redis_cache.redis_cache.hostname}:${azurerm_redis_cache.redis_cache.ssl_port}/0"
        },
        {
          name    = "directus-admin-user-pw"
          value   =  random_password.directus_admin_pw.result
        },
        {
          name    = "directus-storage-secret"
          value   =  random_uuid.directus_secret.result
        }
      ]
    }
    template          = {
      containers      = [{
        image         = "directus/directus:latest"
        name          = "universal-games-directus"
        env           = [{
          name        = "APP_PORT"
          value       = 8055
        },
        {
          name        = "KEY"
          value   = random_uuid.directus_key.result
        },
        {
          name        = "SECRET"
          secretRef   = "directus-storage-secret"
        },
        {
          name        = "ADMIN_EMAIL"
          value       = "it@bcc.no"
        },
        {
          name        = "ADMIN_PASSWORD"
          secretRef   = "directus-admin-user-pw"
        },
        {
          name        = "DB_CLIENT"
          value       = "pg"
        },
        {
          name        = "DB_HOST"
          value       = data.azurerm_postgresql_flexible_server.postgresql_server.fqdn
        },
        {
          name        = "DB_PORT"
          value       = 5432
        },
        {
          name        = "DB_SSL"
          value       = true
        },
        {
          name        = "DB_DATABASE"
          value       = module.postgresql_db.db_name
        },
        {
          name        = "DB_USER"
          value       = module.postgresql_db.db_user_username
        },
        {
          name        = "DB_PASSWORD"
          secretRef   = "postgresql-password"
        },
        {
          name        = "CACHE_ENABLED"
          value       = true
        },
        {
          name        = "CACHE_STORE"
          value       = "redis"
        },
        {
          name        = "CACHE_REDIS"
          secretRef   = "redis-connection-string"
        }
        
        ]
        resources     = {
          cpu         = 0.5
          memory      = "1Gi"
        }
      }]
      scale           = {
        minReplicas   = 0
        maxReplicas   = 1
      }
    }
  }
}

# Add API Route
module "front_door_route" {
  name                 = "universalgames"
  source               = "../../modules/front_door_route"
  resource_group_id    = data.azurerm_resource_group.platform_rg.id
  origin_host          = module.api_container_app.domain_name   
  front_door_name      = "bcc-platform-${terraform.workspace}-frontdoor"
  endpoint_domain_name = "az-api-${terraform.workspace}.bcc.no"
  depends_on = [
    module.api_container_app
  ]
}