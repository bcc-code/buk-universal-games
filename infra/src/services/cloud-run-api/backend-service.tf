# resource "google_compute_region_network_endpoint_group" "default" {
#   name                  = "${var.service-name}-neg"
#   network_endpoint_type = "SERVERLESS"
#   region                = google_cloud_run_service.default.location
#   cloud_run {
#     service = google_cloud_run_service.default.name
#   }
# }

# resource "google_compute_backend_service" "default" {
#   provider = google-beta
#   name     = "${var.service-name}-backend"
#   backend {
#     group           = google_compute_region_network_endpoint_group.default.id
#     balancing_mode  = "UTILIZATION"
#     capacity_scaler = 1
#   }
#   log_config {
#     enable      = true
#     sample_rate = 1
#   }
# }
