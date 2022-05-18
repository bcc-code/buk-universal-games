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

resource "google_project_service" "default" {
  for_each = toset( [
    "iam.googleapis.com", 
    "compute.googleapis.com",
    "run.googleapis.com",
    "artifactregistry.googleapis.com",
    "secretmanager.googleapis.com",
    "sqladmin.googleapis.com",
    "redis.googleapis.com",
    "vpcaccess.googleapis.com",
    "servicenetworking.googleapis.com"
    ] )
  project = var.gcp-project-id
  service = each.key

  disable_dependent_services = true
}