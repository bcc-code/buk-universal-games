resource "google_compute_backend_bucket" "static_backend" {
  name        = "${var.service-name}-backend"
  description = "Backend for static bucket"
  bucket_name = google_storage_bucket.static-site.name
  enable_cdn  = true
}
