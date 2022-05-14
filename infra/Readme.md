# Instructions for setting up infrastructure

## TODO

1. Setup cloud run in new project
2. Deploy container to cloud run (via github actions)

## Requirements

1. [Terraform](https://learn.hashicorp.com/tutorials/terraform/install-cli)
2. [Terragrunt](https://terragrunt.gruntwork.io/docs/getting-started/install/)
3. [gcloud cli](https://cloud.google.com/sdk/docs/install)

## Deploying

1. Login to your Google Cloud account:
    ```
    gcloud auth application-default login
    ```
2. Generate GitHub personal access token
   1. Go to https://github.com/settings/tokens
   2. Click "Generate new token"
   3. Select "repo" scope and generate token
   4. Create file: ```evn/secrets.tfvars``` and paste: 
    
    ```
    github-token = "{YOUR_GENERATED_TOKEN}"
    db-remote-admin-pw = "{PW_FOR_REMOTE_ADMIN}" 
    ```

3. Run the following commands to deploy the infrastructure:
    ```
    ./init_prod.bat 
    ./apply_prod.bat
    ```


## Notes:

* GCP load balancing / certificates quite complicated
* Provisioning of certificate / domain slow