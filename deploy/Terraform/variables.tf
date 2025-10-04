# set TF_VAR in GitHub Actions (https://spacelift.io/blog/terraform-environment-variables)
variable "sql_admin_username" {
  type        = string
  description = "The administrator username of the SQL logical server."
  default     = "azureadmin"
}

variable "sql_admin_password" {
  type        = string
  description = "The administrator password of the SQL logical server."
  sensitive   = true
  default     = null
}

variable "google_client_id" {
  description = "Google OAuth Client ID"
  type        = string
  sensitive   = true
}

variable "google_client_secret" {
  description = "Google OAuth Client Secret"
  type        = string
  sensitive   = true
}

variable "email_from" {
  description = "Email address for sending emails"
  type        = string
  sensitive   = true
}

variable "smtp_password" {
  description = "SMTP password for email sending"
  type        = string
  sensitive   = true
}

variable "content_safety_key" {
  description = "Azure AI foundry content safety key"
  type        = string
  sensitive   = true
}

variable "content_safety_api" {
  description = "Azure AI foundry content safety api endpoint"
  type        = string
}