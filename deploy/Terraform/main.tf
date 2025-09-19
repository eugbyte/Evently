terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.45.0"
    }
  }
}

locals {
  environment  = "staging"
  project_name = "evently"
}
provider "azurerm" {
}

# Define a resource group for your resources
resource "azurerm_resource_group" "rg" {
  name     = "rg-evently-dev-sea"
  location = "southeastasia"
}

resource "azurerm_log_analytics_workspace" "analytics" {
  name                = "log-storeonwheels-prod-sea"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}