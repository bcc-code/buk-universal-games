output "db-name" {
  value = postgresql_database.default.name
}

output "db-username" {
  value = postgresql_role.default.name
}

output "db-password" {
  sensitive = true
  value     = postgresql_role.default.password
}
