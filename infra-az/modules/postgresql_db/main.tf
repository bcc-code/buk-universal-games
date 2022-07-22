terraform {
  required_providers {
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.12.0"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.3.0"
    }
  }
}

locals {
    db_username = var.db_name
    db_name     = var.db_name
}

# Create database
resource "azurerm_postgresql_flexible_server_database" "database" {
  name      = var.db_name
  server_id = var.server_resource_id
  collation = "en_US.utf8"
  charset   = "utf8"
}

# Generate password
resource "random_password" "db_user_password" {
  length           = 32
  special          = true
  override_special = "!#*()-_=+[]:?"
}

# Add admin user
resource "postgresql_role" "db_user" {
  name      = local.db_username
  login     = true
  password  = random_password.db_user_password.result
  depends_on = [
    azurerm_postgresql_flexible_server_database.database
  ]
}

resource "postgresql_grant" "revoke-default" {
  database    = local.db_name
  role        = "public"
  object_type = "database"
  privileges  = []
  depends_on = [
    azurerm_postgresql_flexible_server_database.database
  ]
}

resource "postgresql_grant" "admin-access" {
  database    = var.db_name
  role        = postgresql_role.db_user.name
  object_type = "database"
  privileges  = ["CREATE", "CONNECT"]
  depends_on = [
    azurerm_postgresql_flexible_server_database.database
  ]
}