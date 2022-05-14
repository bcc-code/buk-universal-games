variable "gcp-location" {
  type = string
}
variable "environment-name" {
  type = string
}

variable "service-name" {
  type = string
}

variable "domain-name" {
  type = string
}

variable "cors" {
  default = []
  type    = list(string)
}
