resource "google_compute_network" "vpc" {
  name                    = local.serverless-network-name
  auto_create_subnetworks = false
}

resource "google_compute_subnetwork" "serverless-subnet" {
  name          = local.serverless-subnet-name
  ip_cidr_range = "10.10.0.0/28"
  region        = var.gcp-location
  network       = google_compute_network.vpc.id
  private_ip_google_access = true
  description           = "Serverless (e.g. Cloud Run) VPC Connector Subnet"
}

