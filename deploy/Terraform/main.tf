terraform {
  required_version = ">= 1.0.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
  }
}

locals {
  environment = "staging"
  project_name = "evently"
}
provider "azurerm" {
  features {}
}
