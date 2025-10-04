# Budget for the resource group
data "azurerm_client_config" "current" {}

resource "azurerm_consumption_budget_subscription" "evently_budget" {
  name            = "budget-evently-dev"
  subscription_id = "/subscriptions/${data.azurerm_client_config.current.subscription_id}"

  amount     = 100 # 100 USD
  time_grain = "Monthly"

  time_period {
    start_date = formatdate("YYYY-MM-01T00:00:00Z", plantimestamp())
  }

  notification {
    enabled   = true
    threshold = 80 // send notification when 80% reached
    operator  = "GreaterThan"

    contact_emails = [
      "eugenetham1994@gmail.com"
    ]

    contact_groups = []
    contact_roles = [
      "Owner",
      "Contributor"
    ]
  }

  notification {
    enabled   = true
    threshold = 100
    operator  = "GreaterThan"

    contact_emails = [
      "eugenetham1994@gmail.com"
    ]

    contact_groups = []
    contact_roles = [
      "Owner",
      "Contributor"
    ]
  }
}
