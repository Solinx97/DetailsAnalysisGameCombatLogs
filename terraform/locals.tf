locals {
    env = "dev"
    region = "westus2"
    resource_group_name = "combat-analysis"
    eks_name = "encounters-analysis"
    eks_version = "1.27"
    sql_server_name = "mysolinxserver"
    user_db_name = "Combat_Analysis.User"
    identity_db_name = "Combat_Analysis.Identit"
    chat_db_name = "Combat_Analysis.Chat"
    combat_logs_db_name = "Combat_Analysis.Combat_Logs"
    communication_db_name = "Combat_Analysis.Communication"
    sql_administrator_login = "oleg123"
    sql_administrator_login_password = "der45-jui8n-7hjkl-9iuyt"
}