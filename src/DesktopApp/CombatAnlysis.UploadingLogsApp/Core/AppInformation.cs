using CombatAnalysis.UploadingLogsApp.Enums;

namespace CombatAnalysis.Core.Core;

internal static class AppInformation
{
    public static string Name { get; set; } = string.Empty;

    public static AppVersionType VersionType { get; set; }

    public static string Version { get; set; } = string.Empty;
}
