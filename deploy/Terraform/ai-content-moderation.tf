# Azure AI Foundry Content Safety resource
resource "azurerm_cognitive_account" "content_safety" {
  name                = "cs-evently-dev-sea"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "ContentSafety"
  sku_name            = "F0"
}