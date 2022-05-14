
output "bucket" {
  value = google_storage_bucket.static-site
}


output "backend-service" {
  value = google_compute_backend_bucket.static_backend
}
