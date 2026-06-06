using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities;

public class CombatPlayerPreAura : ICombatPlayerEntity
{
    public string CreatorGameId { get; set; }

    public int GameId { get; set; }

    public int Status { get; set; }

    public int CombatPlayerId { get; set; }
}
