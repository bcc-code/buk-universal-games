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
  }
  experiments = [module_variable_optional_attrs]

  backend "azurerm" {
    resource_group_name  = "BCC-Platform"
    storage_account_name = "bccplatformtfstate"
    container_name       = "tfstate"
    key                  = "bcc-platform-postgresql.terraform.tfstate"    
  }
}

locals {
    location        = "WestEurope"
    resource_group  = "BCC-Platform-${terraform.workspace}"
    resource_prefix = "bcc-platform-${terraform.workspace}" 
    tags            = {}
}

provider "azurerm" {
  features {}
}

provider "azapi" {
}

data "azurerm_client_config" "current" {}

# Vault for postgresql
resource "azurerm_key_vault" "postgresql_vault" {
  name                = lower(replace("${local.resource_prefix}-psql","-",""))
  location            = local.location
  resource_group_name = local.resource_group
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = "standard"
  purge_protection_enabled = true
  enabled_for_disk_encryption = true
  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id
    key_permissions = [
      "Get", "UnwrapKey", "WrapKey"
    ]
    secret_permissions = [
      "Get", "Backup", "Delete", "List", "Purge", "Recover", "Restore", "Set",
    ]
    storage_permissions = [
      "Get"
    ]
  }
}

# Admin Password for Postgresql
resource "random_password" "postgreql_admin_password" {
  length           = 16
  special          = true
  override_special = "!#$%&*()-_=+[]{}<>:?"
}

# Store admin password in Azure Key vault
resource "azurerm_key_vault_secret" "postgreql_admin_password" {
  name         = "postgreql-admin-password"
  value        = random_password.postgreql_admin_password.result
  key_vault_id = azurerm_key_vault.postgresql_vault.id
  depends_on = [ azurerm_key_vault.postgresql_vault ]
}

# Postgresql server
resource "azurerm_postgresql_flexible_server" "postgresql" {
  name                = "${local.resource_prefix}-postgresql"
  location            = local.location
  resource_group_name = local.resource_group

  administrator_login          = "psqladmin"
  administrator_password       = azurerm_key_vault_secret.postgreql_admin_password.value

  sku_name                     = "B_Standard_B1ms"
  version                      = "13"
  storage_mb                   = 32768

  backup_retention_days        = 35
  geo_redundant_backup_enabled = false
  create_mode = "Default"

  tags = local.tags

  # delegated_subnet_id = 
  # private_dns_zone_id =

}

# Add firewall rule
resource "azurerm_postgresql_flexible_server_firewall_rule" "rvo_home" {
  name             = "rvo_home"
  server_id        = azurerm_postgresql_flexible_server.postgresql.id
  start_ip_address = "81.166.215.202"
  end_ip_address   = "81.166.215.202"
}

data "http" "myip" {
  url = "http://ipv4.icanhazip.com"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "terraform_deploy_ip" {
  name             = "terraform_deploy_ip"
  server_id        = azurerm_postgresql_flexible_server.postgresql.id
  start_ip_address = "${chomp(data.http.myip.body)}"
  end_ip_address   = "${chomp(data.http.myip.body)}"
}