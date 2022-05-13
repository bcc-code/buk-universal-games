terraform {
  required_providers {
    google = {
      source = "hashicorp/google"
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

module "buk-universal-games-api" {
  source                       = "./cloud-run-api"
  service-name                 = local.api-service-name
  # sql-instance-connection-name = module.postgres-instance.connection-name
  gcp-location                 = var.gcp-location
  environment-name             = var.environment-name
  service-account-email        = google_service_account.github-build.email
  environment-secrets = {
  }
  environment-variables = {
  }
}
