variable "application_name" {
  type = string
}

variable "primary_location" {
  type = string
}

variable "primary_location_short" {
  type = string
}

variable "subscription_id" {
  type = string
}

variable "postgres_password" {
  type      = string
  sensitive = true
}

# environment specific
variable "environment_name" {
  type = string
}

variable "storage_account_replication_type" {
  type = string
}

variable "storage_account_tier" {
  type = string
}

variable "service_plan_sku_name" {
  type = string
}

variable "app_always_on" {
  type = bool
}

variable "psql_storage_mb" {
  type = string
}

variable "psql_storage_tier" {
  type = string
}

variable "psql_sku_name" {
  type = string
}

variable "key_vault_sku_name" {
  type = string
}
