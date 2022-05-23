resource "google_sql_database_instance" "default" {
  name             = "${var.instance-name}-${var.environment-name}"
  database_version = "POSTGRES_13"
  region           = var.gcp-location

  settings {
    tier = var.db-tier
    backup_configuration {
      enabled                        = true
      location                       = "europe-west3"
      point_in_time_recovery_enabled = true
    }
  }

}

resource "random_password" "terraform-user-password" {
  length           = 32
  special          = true
  override_special = "!#*()-_=+[]:?"
}

resource "random_password" "remote-admin-password" {
  length           = 32
  special          = true
  override_special = "!#*()-_=+[]:?"
}

resource "google_sql_user" "terraform-user" {
  name     = "terraform-user"
  instance = google_sql_database_instance.default.name
  password = random_password.terraform-user-password.result
}

resource "google_sql_user" "remote-admin" {
  name     = "remote-admin"
  instance = google_sql_database_instance.default.name
  password = coalesce(var.db-remote-admin-pw, random_password.remote-admin-password.result) # Ensure password isn't blank!
}
