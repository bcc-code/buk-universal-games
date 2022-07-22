

# Create application: azuread_application

## Owner: current user + BCC IT group

# Create service principal for application: azuread_application_password (client credentials



https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/guides/managed_service_identity
data "azurerm_role_definition" "contributor" {
  name = "Contributor"
}

resource "azurerm_role_assignment" "example" {
  name               = azurerm_virtual_machine.example.name
  scope              = data.azurerm_subscription.primary.id
  role_definition_id = "${data.azurerm_subscription.subscription.id}${data.azurerm_role_definition.contributor.id}"
  principal_id       = azurerm_virtual_machine.example.identity[0]["principal_id"]
}   

https://blog.johnnyreilly.com/2021/12/27/azure-container-apps-build-and-deploy-with-bicep-and-github-actions
az ad sp create-for-rbac --name "myApp" --role contributor \
    --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group} \
    --sdk-auth)

