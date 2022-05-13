resource "google_secret_manager_secret" "default" {
  for_each  = var.environment-secrets
  secret_id = "${var.service-name}-${each.key}"
  replication {
    automatic = true
  }
}

resource "google_secret_manager_secret_version" "default" {
  for_each    = google_secret_manager_secret.default
  secret      = each.value.id
  secret_data = var.environment-secrets[each.key]
}
