resource "google_artifact_registry_repository" "default" {
  provider = google-beta

  location      = var.gcp-location
  repository_id = "default-${var.environment-name}"
  description   = "Default repository for ${var.environment-name}"
  format        = "DOCKER"
}
