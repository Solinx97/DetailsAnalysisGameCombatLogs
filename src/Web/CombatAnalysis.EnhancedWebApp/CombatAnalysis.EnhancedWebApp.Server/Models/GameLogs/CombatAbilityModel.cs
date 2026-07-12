namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatAbilityModel
{
    public int Id { get; set; }

    public int GameId { get; set; }

    public string Name { get; set; }

    public int AbilityType { get; set; }
}
