namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class BossModel
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string Name { get; set; } = string.Empty;

    public long Health { get; set; }

    public int Difficult { get; set; }

    public int Size { get; set; }

    public int BossMapId { get; set; }
}
