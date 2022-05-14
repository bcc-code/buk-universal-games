resource "google_service_account" "service_account" {
  account_id   = "remote-admin"
  display_name = "${var.gcp-project-id}-remote-admin"
}

resource "google_project_iam_member" "service-account-cloudsql-admin-role" {
  project = var.gcp-project-id
  role    = "roles/cloudsql.admin"
  member  = "serviceAccount:${google_service_account.service_account.email}"
}