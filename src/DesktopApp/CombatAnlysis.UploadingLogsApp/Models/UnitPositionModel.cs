using System;

namespace CombatAnalysis.UploadingLogsApp.Models;

public class UnitPositionModel
{
    public string Id { get; set; } = string.Empty;

    public string OwnerGameId { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    public TimeSpan Time { get; set; }

    public string CombatUnitId { get; set; } = string.Empty;
}
