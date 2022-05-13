include "root" {
  path = find_in_parent_folders()
}

include "env" {
  path = "../../_env/project.hcl"
}
