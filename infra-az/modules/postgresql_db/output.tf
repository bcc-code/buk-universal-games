output "db_name" {
  value = local.db_name
}

output "db_user_username" {
  value = local.db_username
}

output "db_user_password" {
  value     = random_password.db_user_password.result
  sensitive = true
}