using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerPreAura : CombatPlayerDataBase
{
    private CombatPlayerPreAura() { }

    public CombatPlayerPreAura(string creatorGameId, int gameId, int status, int combatPlayerId)
    {
        CreatorGameId = creatorGameId;
        GameId = gameId;
        Status = status;
        CombatPlayerId = combatPlayerId;
    }

    public string CreatorGameId { get; private set; }

    public int GameId { get; private set; }

    public int Status { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}
