namespace CombatAnalysis.Core.Models.GameLogs;

public class BossMapModel
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string Name { get; set; } = string.Empty;

    public double X0 { get; set; }

    public double X1 { get; set; }

    public double Y0 { get; set; }

    public double Y1 { get; set; }
}
