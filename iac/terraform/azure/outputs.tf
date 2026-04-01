output "rand_suffix" {
  value = random_string.suffix.result
}

output "location" {
  value = azurerm_resource_group.main.location
}

output "tenant_id" {
  value = data.azurerm_client_config.current.tenant_id
}

output "subscription_id" {
  value = data.azurerm_subscription.subscription.subscription_id
}

output "subscription_name" {
  value = data.azurerm_subscription.subscription.display_name
}

output "resource_group_name" {
  value = azurerm_resource_group.main.name
}

output "managed_identity_name" {
  value = azurerm_user_assigned_identity.main.name
}

output "storage_account_name" {
  value = azurerm_storage_account.main.name
}

output "log_analytics_workspace_name" {
  value = azurerm_log_analytics_workspace.main.name
}

output "app-insights_name" {
  value = azurerm_application_insights.main.name
}

output "service_plan_name" {
  value = azurerm_service_plan.main.name
}

output "web_app_name" {
  value = azurerm_linux_web_app.maintenance.name
}

output "function_app_name" {
  value = azurerm_linux_function_app.main.name
}

output "postgres_server_name" {
  value = azurerm_postgresql_flexible_server.main.name
}

output "key_vault_name" {
  value = azurerm_key_vault.main.name
}
