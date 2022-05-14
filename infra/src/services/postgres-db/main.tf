terraform {
  required_providers {
    postgresql = {
      source  = "cyrilgdn/postgresql"
      version = "1.12.0"
    }
  }
}

resource "postgresql_database" "default" {
  name              = var.db-name
  connection_limit  = -1
  allow_connections = true
}

resource "random_password" "default" {
  length           = 32
  special          = true
  override_special = "!#*()-_=+[]:?"
}

resource "postgresql_role" "default" {
  name     = "${var.db-name}-default"
  login    = true
  password = random_password.default.result
}

resource "postgresql_grant" "revoke-default" {
  database    = postgresql_database.default.name
  role        = "public"
  object_type = "database"
  privileges  = []
}

resource "postgresql_grant" "admin-access" {
  database    = postgresql_database.default.name
  role        = postgresql_role.default.name
  object_type = "database"
  privileges  = ["CREATE", "CONNECT"]
}

resource "postgresql_grant" "super-admin-table-access" {
  database    = postgresql_database.default.name
  role        = var.superuser-name
  schema      = "public"
  object_type = "table"
  privileges  = ["SELECT", "INSERT", "UPDATE", "DELETE", "TRUNCATE", "REFERENCES", "TRIGGER"]
}

resource "postgresql_grant" "super-admin-database-access" {
  database    = postgresql_database.default.name
  role        = var.superuser-name
  object_type = "database"
  privileges  = ["CREATE", "CONNECT"]
}
