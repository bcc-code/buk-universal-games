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


module "buk-universal-games-api" {
  source                       = "./cloud-run-api"
  service-name                 = local.api-service-name
  sql-instance-connection-name = module.postgres-instance.connection-name
  gcp-location                 = var.gcp-location
  environment-name             = var.environment-name
  service-account-email        = google_service_account.github-build.email
  environment-secrets = {
    POSTGRES_PASSWORD   = module.buk-universal-games-db.db-password
  }
  environment-variables = {
    POSTGRES_HOST   = "/cloudsql/${module.postgres-instance.connection-name}"
    POSTGRES_PORT   = 5432
    POSTGRES_USER   = module.buk-universal-games-db.db-username
    POSTGRES_DB     = module.buk-universal-games-db.db-name
  }
}
