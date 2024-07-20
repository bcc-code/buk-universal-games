variable "gcp-location" {
  type = string
}
variable "gcp-project-id" {
  type = string
}
variable "environment-name" {
  type = string
}
variable "service-account-email" {
  type = string
}

variable "service-name" {
  type = string
}
variable "max-scale" {
  type    = string
  default = "50"
}

variable "sql-instance-connection-name" {
  nullable = true
  default  = null
  type     = string
}

variable "environment-variables" {
  default = {}
  type    = map(string)
}
variable "environment-secrets" {
  default = {}
  type    = map(string)
}

variable "vpc-serverless-connector-name" {
  type = string
}

variable "docker-image" {
  type = string
  default = "us-docker.pkg.dev/cloudrun/container/hello"
  
}
