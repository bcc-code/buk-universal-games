output "terraform-username" {
  value = google_sql_user.terraform-user.name
}

output "terraform-password" {
  sensitive = true
  value     = google_sql_user.terraform-user.password
}

output "connection-name" {
  value = google_sql_database_instance.default.connection_name
}

output "name" {
  value = google_sql_database_instance.default.name
}
