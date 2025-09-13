# Create a storage account
resource "azurerm_storage_account" "evently_sa" {
  name                     = "saeventlydevsea"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  # Optional: Lifecycle management for blob storage
  blob_properties {
    versioning_enabled = false  # Disable versioning to save costs
    delete_retention_policy {
      days = 7  # Minimum retention period
    }
  }
}

# Create a blob container
resource "azurerm_storage_container" "evently_container" {
  name                  = "evently-dev-images"
  storage_account_id    = azurerm_storage_account.evently_sa.id
  container_access_type = "public"
}