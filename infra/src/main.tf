terraform {
  required_providers {
    google = {
      source = "hashicorp/google"
    }
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.12.0"
    }
    httpclient = {
      version = "0.0.3"
      source  = "dmachard/http-client"
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

module "buk-universal-games-api" {
  source                       = "./cloud-run-api"
  application-name             = "buk-universal-games-api"
  sql-instance-connection-name = module.postgres-instance.connection-name
  gcp-location                 = var.gcp-location
  environment-name             = var.environment-name
  service-account-email        = google_service_account.github-build.email
}
