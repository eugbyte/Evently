# set TF_VAR_sql_admin_username in .env
variable "sql_admin_username" {
  type        = string
  description = "The administrator username of the SQL logical server."
  default     = "azureadmin"
}

# set TF_VAR_sql_admin_password in .env
variable "sql_admin_password" {
  type        = string
  description = "The administrator password of the SQL logical server."
  sensitive   = true
  default     = null
}