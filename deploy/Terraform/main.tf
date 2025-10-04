terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.45.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "rg-sharedinfra-sea" # Resource group for the storage account
    storage_account_name = "sasharedinfrasea"
    container_name       = "evently-dev-sea"
    key                  = "tfstate/terraform.tfstate"
  }
}

locals {
  environment = "staging"
}

provider "azurerm" {
  subscription_id = "e99ca647-4d2d-465f-b0a2-0b97d93c9024"
  features {}
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