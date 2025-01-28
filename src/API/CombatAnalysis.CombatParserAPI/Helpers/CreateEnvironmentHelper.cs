using CombatAnalysis.CombatParserAPI.Consts;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        DatabaseProps.Name = configuration["Database:Name"] ?? string.Empty;
        DatabaseProps.DataProcessingType = configuration["Database:DataProcessingType"] ?? string.Empty;
        DatabaseProps.MSSQLConnectionString = configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
        DatabaseProps.FirebaseConnectionString = configuration["ConnectionStrings:Firebase"] ?? string.Empty;

        if (int.TryParse(configuration["DBConfiguration:CommandTimeout"], out var commandTimeout))
        {
            DBConfiguration.CommandTimeout = commandTimeout;
        }

        if (int.TryParse(configuration["DBConfiguration:MaxRequestBodySize"], out var maxRequestBodySize))
        {
            DBConfiguration.MaxRequestBodySize = maxRequestBodySize;
        }

        var specs = configuration.GetSection("Players:Specs").GetChildren();
        PlayerInfoConfiguration.Specs = specs?.ToDictionary(entry => entry.Key, entry => entry.Value) ?? [];

        var classes = configuration.GetSection("Players:Classes").GetChildren();
        PlayerInfoConfiguration.Classes = classes?.ToDictionary(entry => entry.Key, entry => entry.Value) ?? [];

        var bosses = configuration.GetSection("Players:Bosses").GetChildren();
        PlayerInfoConfiguration.Bosses = bosses?.ToDictionary(entry => entry.Key, entry => entry.Value) ?? [];
    }

    public static void UseEnvVariables()
    {
        DatabaseProps.Name = Environment.GetEnvironmentVariable("Database_Name") ?? string.Empty;
        DatabaseProps.DataProcessingType = Environment.GetEnvironmentVariable("Database_DataProcessingType") ?? string.Empty;
        DatabaseProps.MSSQLConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings_DefaultConnection") ?? string.Empty;
        DatabaseProps.FirebaseConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings_Firebase") ?? string.Empty;

        if (int.TryParse(Environment.GetEnvironmentVariable("DBConfiguration_CommandTimeout"), out var commandTimeout))
        {
            DBConfiguration.CommandTimeout = commandTimeout;
        }

        if (int.TryParse(Environment.GetEnvironmentVariable("DBConfiguration_MaxRequestBodySize"), out var maxRequestBodySize))
        {
            DBConfiguration.MaxRequestBodySize = maxRequestBodySize;
        }

        var specs = Environment.GetEnvironmentVariable("Players_Specs");
        PlayerInfoConfiguration.Specs = SettingsHelper.ConvertToDictionary(specs) ?? [];

        var classes = Environment.GetEnvironmentVariable("Players_Classes");
        PlayerInfoConfiguration.Classes = SettingsHelper.ConvertToDictionary(classes) ?? [];

        var bosses = Environment.GetEnvironmentVariable("Players_Bosses");
        PlayerInfoConfiguration.Bosses = SettingsHelper.ConvertToDictionary(bosses) ?? [];
    }
}
