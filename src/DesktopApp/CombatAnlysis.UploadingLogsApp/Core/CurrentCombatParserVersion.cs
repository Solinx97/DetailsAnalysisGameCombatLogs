using CombatAnalysis.UploadingLogsApp.Enums;

namespace CombatAnalysis.UploadingLogsApp.Core;

internal static class CurrentCombatParserVersion
{
    public static CombatParserVersion Version { get; set; } = CombatParserVersion.WoWMidnight;
}
