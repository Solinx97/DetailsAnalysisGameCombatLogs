resource "azurerm_mssql_server" "sql" {
  name                         = local.sql_server_name
  resource_group_name          = azurerm_resource_group.this.name
  location                     = azurerm_resource_group.this.location
  version                      = "12.0"
  administrator_login          = local.sql_administrator_login
  administrator_login_password = local.sql_administrator_login_password
}

resource "azurerm_mssql_database" "sqluser" {
  name                        = local.user_db_name
  server_id                   = azurerm_mssql_server.sql.id
  min_capacity                = 0.5
  max_size_gb                 = 24
  sku_name                    = "GP_S_Gen5_2"
  auto_pause_delay_in_minutes = 60
}

resource "azurerm_mssql_database" "sqlidentity" {
  name                        = local.identity_db_name
  server_id                   = azurerm_mssql_server.sql.id
  min_capacity                = 0.5
  max_size_gb                 = 24
  sku_name                    = "GP_S_Gen5_2"
  auto_pause_delay_in_minutes = 60
}

resource "azurerm_mssql_database" "sqlchat" {
  name                        = local.chat_db_name
  server_id                   = azurerm_mssql_server.sql.id
  min_capacity                = 0.5
  max_size_gb                 = 24
  sku_name                    = "GP_S_Gen5_2"
  auto_pause_delay_in_minutes = 60
}

resource "azurerm_mssql_database" "sqlcombatlogs" {
  name                        = local.combat_logs_db_name
  server_id                   = azurerm_mssql_server.sql.id
  min_capacity                = 0.5
  max_size_gb                 = 32
  sku_name                    = "GP_S_Gen5_2"
  auto_pause_delay_in_minutes = 60
}

resource "azurerm_mssql_database" "sqlcommunication" {
  name                        = local.communication_db_name
  server_id                   = azurerm_mssql_server.sql.id
  min_capacity                = 0.5
  max_size_gb                 = 24
  sku_name                    = "GP_S_Gen5_2"
  auto_pause_delay_in_minutes = 60
}