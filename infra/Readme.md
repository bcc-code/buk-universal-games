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
   4. Create file: ```src/secrets.auto.tfvars``` and paste: ```github-token = "{YOUR_GENERATED_TOKEN}"```
4. In the repository, navigate to the directory where you want to deploy the infrastructure:
    ```
    cd env/dev
    ```
5. Run the following commands to deploy the infrastructure:
    ```
    terragrunt init
    terragrunt apply
    ```


#### Connect to the Postgres instance

1. Download [Cloud SQL Auth proxy](https://cloud.google.com/sql/docs/mysql/connect-admin-proxy)
2. Start Cloud SQL Auth proxy with
    ```cloud_sql_proxy -instances=<GCP_PROJECT_ID>:<GCP_LOCATION>:<DB_INSTANCE_NAME>=tcp:<PORT>```
3. Connect to the DB using your favourite SQL client, the host is ```localhost:<PORT>```