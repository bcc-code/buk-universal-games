#!/bin/bash

env_name=dev

# Select dev workspace
terraform workspace select $env_name || terraform workspace new $env_name

# Terraform Init
terraform init

# Terraform validate
terraform validate -compact-warnings

# Terraform plan
terraform plan -compact-warnings -out=main-$env_name.tfplan # -var-file=deploy-$env_name.tfvars

# Terraform apply
terraform apply -compact-warnings -auto-approve main-$env_name.tfplan

# Remove plan file
rm main-$env_name.tfplan
