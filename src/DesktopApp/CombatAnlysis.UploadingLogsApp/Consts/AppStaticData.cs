using System.Collections.Generic;

namespace CombatAnalysis.UploadingLogsApp.Consts;

public static class AppStaticData
{
    public static int PreparedCombatsCount { get; set; }

    public static List<string> SelectedCombatLogFilePaths { get; set; } = [];
}