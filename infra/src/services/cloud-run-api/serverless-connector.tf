
module "serverless_connector" {
  source  = "terraform-google-modules/network/google//modules/vpc-serverless-connector-beta"
  version = "5.0.0"

  project_id = var.gcp-project-id
  vpc_connectors = [{
    name            = "vpc-serverless-${var.environment-name}"
    region          = var.gcp-location
    subnet_name     = var.vpc-subnet-name
    host_project_id = var.gcp-project-id
    machine_type    = "e2-micro"
    min_instances   = 2
    max_instances   = 3
  }]
}