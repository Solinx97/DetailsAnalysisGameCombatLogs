using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Entities.CombatPlayerData;

public class CombatPlayerPreAura : ICombatPlayerEntity
{
    public int Id { get; set; }

    public string CreatorGameId { get; set; }

    public int GameId { get; set; }

    public int Status { get; set; }

    public int CombatPlayerId { get; set; }
}
