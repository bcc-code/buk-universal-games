resource "google_redis_instance" "cache" {
  name           = "memory-cache"
  tier           = "STANDARD_HA"
  read_replicas_mode = "READ_REPLICAS_DISABLED"
  memory_size_gb = 1

  maintenance_policy {
        weekly_maintenance_window {
            day      = "SUNDAY"
            start_time {
                hours   = 1
                minutes = 0
                nanos   = 0
                seconds = 0
            }
        }
    }
}
# data "google_compute_network" "redis-network" {
#   name = "redis-test-network"
# }