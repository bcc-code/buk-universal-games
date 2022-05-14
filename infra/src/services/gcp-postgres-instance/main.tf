resource "google_sql_database_instance" "default" {
  name             = "${var.instance-name}-${var.environment-name}"
  database_version = "POSTGRES_13"
  region           = var.gcp-location

  settings {
    tier = var.db-tier
  }
}

resource "random_password" "terraform-user-password" {
  length           = 32
  special          = true
  override_special = "!#*()-_=+[]:?"
}

resource "google_sql_user" "terraform-user" {
  name     = "terraform-user"
  instance = google_sql_database_instance.default.name
  password = random_password.terraform-user-password.result
}
