provider "github" {
  token = var.github-token
  owner = var.repository-owner
}

data "github_repository" "default" {
  name = var.repository-name
}

module "github-secrets" {
  source      = "./github-secrets"
  repository  = data.github_repository.default.name
  secrets = {
    GOOGLE_CREDENTIALS       = base64decode(google_service_account_key.github-build-key.private_key)
    GOOGLE_PROJECT_ID        = var.gcp-project-id
    GOOGLE_REGION            = var.gcp-location
    "${local.environment-name-uppercase}_REGISTRY_BASE_URL"        = "${google_artifact_registry_repository.default.location}-docker.pkg.dev"
    "${local.environment-name-uppercase}_REGISTRY_NAME"            = google_artifact_registry_repository.default.name
    "${local.environment-name-uppercase}_API_SERVICE_NAME"        = "${local.api-service-name}"
    "${local.environment-name-uppercase}_SITE_BUCKET_NAME"        = "${module.buk-universal-games-site.bucket.name}"
    
    # SQL_INSTANCE_CONNECTION  = module.postgres-instance.connection-name
    # ORGS_SERVICE_NAME        = module.orgs-api.service.name
    # ORGS_SERVICE_REGION      = module.orgs-api.service.location
    # ORGS_DB_USERNAME         = module.orgs-db.db-username
    # ORGS_DB_PASSWORD         = module.orgs-db.db-password
    # ORGS_DB_NAME             = module.orgs-db.db-name
    # DOCS_SERVICE_NAME        = module.docs-app.service.name
    # DOCS_SERVICE_REGION      = module.docs-app.service.location
  }
}
