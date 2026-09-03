using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;

public class CombatPlayerPreAura : ICombatPlayerEntity
{
    public int Id { get; set; }

    public string CreatorGameId { get; set; }

    public int GameId { get; set; }

    public int Status { get; set; }

    public int CombatPlayerId { get; set; }
}
