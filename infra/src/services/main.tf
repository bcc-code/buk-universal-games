terraform {
  required_providers {
    google = {
      source = "hashicorp/google"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.12.0"
    }
  }
  backend "gcs" {}
}

provider "google-beta" {
  project = var.gcp-project-id
  region  = var.gcp-location
}

provider "google" {
  project = var.gcp-project-id
  region  = var.gcp-location
}

locals {
  environment-name-uppercase = upper(var.environment-name)
  api-service-name = "buk-universal-games-api-${var.environment-name}"
  site-service-name = "buk-universal-games-ui-${var.environment-name}"
  directus-service-name = "buk-universal-games-directus-${var.environment-name}"
}

module "postgres-instance" {
  source                = "./gcp-postgres-instance"
  instance-name         = "buk-universal-games-postgres"
  gcp-location          = var.gcp-location
  environment-name      = var.environment-name
  service-account-email = google_service_account.github-build.email
  db-tier               = var.db-tier
  db-remote-admin-pw    = var.db-remote-admin-pw
}

provider "postgresql" {
  scheme    = "gcppostgres"
  superuser = false
  host      = module.postgres-instance.connection-name
  username  = module.postgres-instance.terraform-username
  password  = module.postgres-instance.terraform-password
}

module "buk-universal-games-db" {
  source         = "./postgres-db"
  db-name        = "buk-universal-games"
  superuser-name = module.postgres-instance.terraform-username
}

module "redis-cache" {
  source                       = "./redis-cache"
}


module "buk-universal-games-site" {
  source                       = "./gcp-static-site"
  service-name                 = local.site-service-name
  gcp-location                 = var.gcp-location
  environment-name             = var.environment-name
  domain-name                  = var.domain-name
  cors                         = ["https://${var.domain-name}", "${module.buk-universal-games-api.service.status[0].url}"]
}

resource "google_vpc_access_connector" "vpc-connector" {
  provider        = google-beta
  name            = "vpc-serverless-${var.environment-name}"
  project         = var.gcp-project-id
  # region          = var.gcp-location
  ip_cidr_range   = "10.8.0.0/28" # var.vpc-ip-range
  network         = "default" # var.vpc-network-name
  machine_type    = "e2-micro"
  min_instances   = 2
  max_instances   = 3
}


module "buk-universal-games-api" {
  source                       = "./cloud-run-api"
  service-name                 = local.api-service-name
  sql-instance-connection-name = module.postgres-instance.connection-name
  vpc-serverless-connector-name = google_vpc_access_connector.vpc-connector.name
  gcp-location                 = var.gcp-location
  gcp-project-id               = var.gcp-project-id
  environment-name             = var.environment-name
  service-account-email        = google_service_account.github-build.email
  environment-secrets = {
    POSTGRES_PASSWORD   = module.buk-universal-games-db.db-password
  }
  environment-variables = {
    POSTGRES_HOST     = "/cloudsql/${module.postgres-instance.connection-name}"
    POSTGRES_PORT     = 5432
    POSTGRES_USER     = module.buk-universal-games-db.db-username
    POSTGRES_DB       = module.buk-universal-games-db.db-name
    ENVIRONMENT_NAME  = var.environment-name
    REDIS_CONNECTION_STRING = "${module.redis-cache.service.host}:${module.redis-cache.service.port}"
  }
}

resource "random_uuid" "directus-key" {
}
resource "random_uuid" "directus-secret" {
}


module "buk-universal-games-directus" {
  source                       = "./cloud-run-api"
  service-name                 = local.directus-service-name
  docker-image                 = "directus/directus:latest"
  sql-instance-connection-name = module.postgres-instance.connection-name
  vpc-serverless-connector-name = google_vpc_access_connector.vpc-connector.name
  gcp-location                 = var.gcp-location
  gcp-project-id               = var.gcp-project-id
  environment-name             = var.environment-name
  service-account-email        = google_service_account.github-build.email
  environment-secrets = {
    DB_PASSWORD       = module.buk-universal-games-db.db-password
    ADMIN_PASSWORD    = var.db-remote-admin-pw
  }
  environment-variables = {
    KEY               = random_uuid.directus-key.result
    SECRET            = random_uuid.directus-secret.result
    ADMIN_EMAIL       = "admin@admin.com"    
    DB_CLIENT         = "pg"
    DB_HOST           = "/cloudsql/${module.postgres-instance.connection-name}"
    DB_PORT           = 5432
    DB_USER           = module.buk-universal-games-db.db-username
    DB_DATABASE       = module.buk-universal-games-db.db-name
    CACHE_ENABLED     = true
    CACHE_STORE       = "redis"
    CACHE_REDIS       = "redis://{module.redis-cache.service.host}:${module.redis-cache.service.port}"
    PUBLIC_URL        = "https://${var.domain-name}/directus"
  }
}
