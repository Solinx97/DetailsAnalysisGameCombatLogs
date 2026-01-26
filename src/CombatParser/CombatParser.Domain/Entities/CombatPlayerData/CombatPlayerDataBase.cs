namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerDataBase
{
    public int CombatPlayerId { get; protected set; }

    public void SetCombatPlayerId(int combatPlayerId)
    {
        CombatPlayerId = combatPlayerId;
    }
}
