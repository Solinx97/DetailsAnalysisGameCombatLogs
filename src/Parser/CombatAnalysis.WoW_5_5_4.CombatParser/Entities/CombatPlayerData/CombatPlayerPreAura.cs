using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Entities.CombatPlayerData;

public class CombatPlayerPreAura : ICombatPlayerEntity
{
    public int Id { get; set; }

    public string CreatorGameId { get; set; }

    public int GameId { get; set; }

    public int Status { get; set; }

    public int CombatPlayerId { get; set; }
}
