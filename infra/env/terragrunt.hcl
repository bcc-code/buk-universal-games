terraform {
  source = "${path_relative_from_include()}/../src"
}

remote_state {
  backend = "gcs"
  config = {
    bucket = "buk-universal-games-terraform-state"
    prefix = path_relative_to_include()
  }
}

inputs = {
  environment-name = path_relative_to_include()
}