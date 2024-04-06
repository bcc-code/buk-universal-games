terraform {

  extra_arguments "custom_vars" {
    commands = [
      "apply",
      "plan",
      "import",
      "push",
      "refresh"
    ]

    required_var_files = ["${get_parent_terragrunt_dir()}/common.tfvars", "${get_parent_terragrunt_dir()}/secrets.tfvars"]
  }
}

remote_state {
  backend = "gcs"
  config = {
    bucket = "universal-games-ioc-state"
    prefix = "universal-games_${replace(path_relative_to_include(),"/","-")}"
  }
}

inputs = {
  environment-name =  element(split("/", path_relative_to_include()),0)
}
