namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;

public class GuildDto
{
    public string Name { get; set; }

    public RealmDto Realm { get; set; }

    public FactionDto Faction { get; set; }
}
