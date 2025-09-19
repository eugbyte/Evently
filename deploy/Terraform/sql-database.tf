resource "random_password" "admin_password" {
  count       = var.sql_admin_password == null ? 1 : 0
  length      = 20
  special     = true
  min_numeric = 1
  min_upper   = 1
  min_lower   = 1
  min_special = 1
}

locals {
  admin_password = try(random_password.admin_password[0].result, var.sql_admin_password)
}

resource "azurerm_mssql_server" "sql_server" {
  name                         = "sql-evently-prod-sea"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  administrator_login          = var.sql_admin_username
  administrator_login_password = local.admin_password
  version                      = "12.0"
}

resource "azurerm_mssql_database" "db" {
  name      = "evently"
  server_id = azurerm_mssql_server.sql_server.id
}

# Construct the SQL connection string
# https://blog.simontimms.com/2021/07/26/build_sql_connection_string/  
locals {
  sql_connection_string = "Server=tcp:${azurerm_mssql_server.sql_server.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.db.name};Persist Security Info=False;User ID=${azurerm_mssql_server.sql_server.administrator_login};Password=${azurerm_mssql_server.sql_server.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
}