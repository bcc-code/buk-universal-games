variable "gcp-project-id" {
  type = string
}

variable "gcp-location" {
  type = string
}

variable "environment-name" {
  type = string
}

variable "repository-name" {
  type = string
}
variable "repository-owner" {
  type = string
}

variable "github-token" {
  sensitive = true
}

variable "db-tier" {
  type = string
}

variable "db-remote-admin-pw" {
  type = string
  sensitive   = true
}

variable "domain-name" {
  type = string
}



