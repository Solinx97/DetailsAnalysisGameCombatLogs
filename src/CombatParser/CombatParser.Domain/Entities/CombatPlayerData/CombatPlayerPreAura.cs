using CombatParser.Domain.Entities.Base;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerPreAura : CombatPlayerDataBase
{
    private CombatPlayerPreAura() { }

    private CombatPlayerPreAura(string creatorGameId, int gameId, int status)
    {
        CreatorGameId = creatorGameId;
        GameId = gameId;
        Status = status;
    }

    public string CreatorGameId { get; private set; }

    public int GameId { get; private set; }

    public int Status { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public static CombatPlayerPreAura Create(string creatorGameId, int gameId, int status)
    {
        ArgumentException.ThrowIfNullOrEmpty(creatorGameId, nameof(creatorGameId));

        return new CombatPlayerPreAura(creatorGameId, gameId, status);
    }
}
