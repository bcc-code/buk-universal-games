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
  serverless-network-name = "buk-universal-games-vpc-network"
  serverless-subnet-name = "buk-universal-games-vpc-subnet"
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

module "buk-universal-games-api" {
  source                       = "./cloud-run-api"
  service-name                 = local.api-service-name
  sql-instance-connection-name = module.postgres-instance.connection-name
  gcp-location                 = var.gcp-location
  gcp-project-id               = var.gcp-project-id
  environment-name             = var.environment-name
  service-account-email        = google_service_account.github-build.email
  vpc-subnet-name              = local.serverless-subnet-name
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
  depends_on = [
    google_compute_subnetwork.serverless-subnet
  ]
}
