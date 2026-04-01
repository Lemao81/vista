locals {
  tag_application = "application"
  name_suffix     = "${var.application_name}-${var.environment_name}-${var.primary_location}-${random_string.suffix.result}"
  dotnet_version  = "10.0"
}

data "azurerm_client_config" "current" {}

data "azurerm_subscription" "subscription" {
  subscription_id = var.subscription_id
}

resource "random_string" "suffix" {
  length  = 5
  upper   = false
  special = false
}

resource "azurerm_resource_group" "main" {
  name     = "rg-${local.name_suffix}"
  location = var.primary_location
  tags = {
    (local.tag_application) = var.application_name
  }
}

resource "azurerm_user_assigned_identity" "main" {
  name                = "id-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  tags = {
    (local.tag_application) = var.application_name
  }
}

resource "azurerm_storage_account" "main" {
  name                     = "sa${var.application_name}${var.environment_name}${var.primary_location_short}${random_string.suffix.result}"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_replication_type = var.storage_account_replication_type
  account_tier             = var.storage_account_tier
  tags = {
    (local.tag_application) = var.application_name
  }
}

resource "azurerm_log_analytics_workspace" "main" {
  name                = "log-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  tags = {
    (local.tag_application) = var.application_name
  }
}

resource "azurerm_application_insights" "main" {
  name                = "appi-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  application_type    = "web"
  workspace_id        = azurerm_log_analytics_workspace.main.id
  tags = {
    (local.tag_application) = var.application_name
  }
}

resource "azurerm_service_plan" "main" {
  name                = "asp-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Linux"
  sku_name            = var.service_plan_sku_name
  tags = {
    (local.tag_application) = var.application_name
  }
}

resource "azurerm_linux_web_app" "main" {
  name                = "app-${local.name_suffix}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  service_plan_id     = azurerm_service_plan.main.id

  site_config {
    always_on = var.app_always_on

    application_stack {
      dotnet_version = local.dotnet_version
    }
  }
}

resource "azurerm_linux_function_app" "main" {
  name                       = "func-${local.name_suffix}"
  resource_group_name        = azurerm_resource_group.main.name
  location                   = azurerm_resource_group.main.location
  service_plan_id            = azurerm_service_plan.main.id
  storage_account_name       = azurerm_storage_account.main.name
  storage_account_access_key = azurerm_storage_account.main.primary_access_key
  tags = {
    (local.tag_application) = var.application_name
  }

  site_config {
    always_on                              = var.app_always_on
    application_insights_connection_string = azurerm_application_insights.main.connection_string
    application_insights_key               = azurerm_application_insights.main.instrumentation_key

    application_stack {
      dotnet_version              = local.dotnet_version
      use_dotnet_isolated_runtime = true
    }
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.main.id]
  }

  app_settings = {
    AZURE_CLIENT_ID = azurerm_user_assigned_identity.main.client_id
  }
}

resource "azurerm_postgresql_flexible_server" "main" {
  name                   = "psql-${local.name_suffix}"
  resource_group_name    = azurerm_resource_group.main.name
  location               = azurerm_resource_group.main.location
  version                = "18"
  administrator_login    = "psqladmin"
  administrator_password = var.postgres_password
  storage_mb             = var.psql_storage_mb
  storage_tier           = var.psql_storage_tier
  sku_name               = var.psql_sku_name
  zone                   = "1"
  tags = {
    (local.tag_application) = var.application_name
  }
}

resource "azurerm_key_vault" "main" {
  name                       = "kv-${var.application_name}-${var.environment_name}-${var.primary_location_short}-${random_string.suffix.result}"
  resource_group_name        = azurerm_resource_group.main.name
  location                   = azurerm_resource_group.main.location
  sku_name                   = var.key_vault_sku_name
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days = 7
  rbac_authorization_enabled = true
  tags = {
    (local.tag_application) = var.application_name
  }
}
