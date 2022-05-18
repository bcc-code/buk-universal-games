data "google_project" "project" {
  provider = google-beta
}

resource "google_cloud_run_service" "default" {
  name     = var.service-name
  location = var.gcp-location

  template {
    spec {
      containers {
        image = var.docker-image

        dynamic "env" {
          for_each = google_secret_manager_secret.default

          content {
            name = env.key
            value_from {
              secret_key_ref {
                name = env.value.secret_id
                key  = "latest"
              }
            }
          }
        }
        dynamic "env" {
          for_each = var.environment-variables

          content {
            name  = env.key
            value = env.value
          }
        }
      }
    }
    metadata {
      annotations = {
        "autoscaling.knative.dev/maxScale"        = var.max-scale
        "run.googleapis.com/cloudsql-instances"   = var.sql-instance-connection-name
        "run.googleapis.com/vpc-access-connector" = var.vpc-serverless-connector-name
        "run.googleapis.com/vpc-access-egress"    = "private-ranges-only"
      }
    }
  }

  lifecycle {
    ignore_changes = [
      template[0].metadata[0].annotations["client.knative.dev/user-image"],
      template[0].metadata[0].annotations["run.googleapis.com/client-name"],
      template[0].metadata[0].annotations["run.googleapis.com/client-version"],
      template[0].metadata[0].annotations["run.googleapis.com/sandbox"],
      metadata[0].annotations["client.knative.dev/user-image"],
      metadata[0].annotations["run.googleapis.com/client-name"],
      metadata[0].annotations["run.googleapis.com/client-version"],
      metadata[0].annotations["run.googleapis.com/launch-stage"],
      metadata[0].annotations["serving.knative.dev/creator"],
      metadata[0].annotations["serving.knative.dev/lastModifier"],
      metadata[0].annotations["run.googleapis.com/ingress-status"],
      metadata[0].labels["cloud.googleapis.com/location"],
      template[0].spec[0].containers[0].image
    ]
  }

  traffic {
    percent         = 100
    latest_revision = true
  }
  autogenerate_revision_name = true
  depends_on                 = [
    google_secret_manager_secret_iam_member.secret-access
  ]
}

resource "google_cloud_run_service_iam_member" "noauth" {
  location = google_cloud_run_service.default.location
  service  = google_cloud_run_service.default.name
  role     = "roles/run.invoker"
  member   = "allUsers"
}

resource "google_cloud_run_service_iam_member" "member" {
  location = google_cloud_run_service.default.location
  service  = google_cloud_run_service.default.name
  role     = "roles/owner"
  member   = "serviceAccount:${var.service-account-email}"
}

resource "google_secret_manager_secret_iam_member" "secret-access" {
  for_each  = google_secret_manager_secret.default
  secret_id = each.value.id
  role      = "roles/secretmanager.secretAccessor"
  member    = "serviceAccount:${data.google_project.project.number}-compute@developer.gserviceaccount.com"
}
