using CombatParser.Domain.Entities.Base;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class UnitPreAura : CombatPlayerUnitDataBase
{
    private UnitPreAura() { }

    private UnitPreAura(int gameId, int status, string targetGameId)
    {
        Id = Guid.NewGuid().ToString();
        GameId = gameId;
        Status = status;
        TargetGameId = targetGameId;
    }

    public int GameId { get; private set; }

    public int Status { get; private set; }

    public static UnitPreAura Create(int gameId, int status, string targetGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(targetGameId, nameof(targetGameId));

        return new UnitPreAura(gameId, status, targetGameId);
    }
}
