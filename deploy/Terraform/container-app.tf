# Azure Container Registry (ACR)
resource "azurerm_container_registry" "acr" {
  name                = "acreventlyprodsea"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Basic" # Cheapest option
  admin_enabled       = true
}

locals {
  acrimage = "${azurerm_container_registry.acr.login_server}/eugbyte/evently"
}

# Build and push the image
resource "null_resource" "publish_docker_image" {
  triggers = {
    always_run = timestamp()
  }

  provisioner "local-exec" {
    working_dir = "../.."
    command     = "docker compose build evently"
    environment = {
      IMAGE = "eugbyte/evently:latest"
    }
  }

  provisioner "local-exec" {
    command = "docker tag eugbyte/evently:latest ${local.acrimage}:latest"
  }

  provisioner "local-exec" {
    command = "docker image ls"
  }

  provisioner "local-exec" {
    command = "az acr login --name ${azurerm_container_registry.acr.name}"
  }

  provisioner "local-exec" {
    command = "docker push ${local.acrimage}:latest"
  }

  provisioner "local-exec" {
    command = "docker image prune --force"
  }
  depends_on = [azurerm_container_registry.acr]
}

resource "azurerm_container_app_environment" "env" {
  name                       = "ca-env-evently-prod-sea"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.analytics.id
}

resource "azurerm_user_assigned_identity" "uami" {
  name                = "uami-evently-prod-sea"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
}

resource "azurerm_role_assignment" "acr_pull" {
  principal_id         = azurerm_user_assigned_identity.uami.principal_id
  role_definition_name = "AcrPull"
  scope                = azurerm_container_registry.acr.id
}

locals {
  timestamp = formatdate("YYYYMMDDhhmmss", timestamp())
}

resource "azurerm_container_app" "app" {
  name                         = "ca-evently-prod-sea"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  # needed for authentication when pulling from private ACR 
  # https://azureway.cloud/azure-container-apps-creating-using-terraform-part-1/
  registry {
    server   = azurerm_container_registry.acr.login_server
    identity = azurerm_user_assigned_identity.uami.id
  }

  # needed for container app to access other Microsoft Entra protected resources
  # https://learn.microsoft.com/en-us/azure/container-apps/managed-identity?tabs=portal%2Cdotnet
  identity {
    type = "UserAssigned"
    identity_ids = [
      azurerm_user_assigned_identity.uami.id
    ]
  }

  ingress {
    allow_insecure_connections = false
    external_enabled           = true
    target_port                = 8080
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }

  template {
    revision_suffix = "rev-${local.timestamp}"

    # Minimum scaling for cost optimization
    min_replicas = 0 # Scale to zero when no traffic (cheapest option)
    max_replicas = 1 # Limit maximum instances

    container {
      name  = "eventlyserver"
      image = "${local.acrimage}:latest"
      # Minimum possible resource allocation
      cpu    = 0.25
      memory = "0.5Gi"

      # Database Connection
      env {
        name        = "ConnectionStrings__WebApiDatabase"
        secret_name = "sql-connection-string"
      }

      # Storage Account Configuration
      env {
        name        = "StorageAccount__AzureStorageConnectionString"
        secret_name = "sa-connection-string"
      }

      env {
        name  = "StorageAccount__AccountName"
        value = azurerm_storage_account.evently_sa.name
      }

      # Authentication - Google OAuth
      env {
        name        = "Authentication__Google__ClientId"
        secret_name = "google-client-id"
      }

      env {
        name        = "Authentication__Google__ClientSecret"
        secret_name = "google-client-secret"
      }

      # Email Settings
      env {
        name        = "EmailSettings__ActualFrom"
        secret_name = "email-from"
      }

      env {
        name        = "EmailSettings__SmtpPassword"
        secret_name = "smtp-password"
      }

      # Logging Configuration
      env {
        name  = "Logging__LogLevel__Default"
        value = "Information"
      }

      env {
        name  = "Logging__LogLevel__Microsoft.AspNetCore"
        value = "Warning"
      }

      # General Settings
      env {
        name  = "AllowedHosts"
        value = "*"
      }

      # Environment indicator
      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
    }
  }

  # Database connection string (from SQL database resource)
  secret {
    name  = "sql-connection-string"
    value = local.sql_connection_string
  }

  # Storage account connection string
  secret {
    name  = "sa-connection-string"
    value = azurerm_storage_account.evently_sa.primary_connection_string
  }

  # Google OAuth secrets (these should come from GitHub Secrets via Terraform variables)
  secret {
    name  = "google-client-id"
    value = var.google_client_id
  }

  secret {
    name  = "google-client-secret"
    value = var.google_client_secret
  }

  # Email configuration secrets
  secret {
    name  = "email-from"
    value = var.email_from
  }

  secret {
    name  = "smtp-password"
    value = var.smtp_password
  }

}