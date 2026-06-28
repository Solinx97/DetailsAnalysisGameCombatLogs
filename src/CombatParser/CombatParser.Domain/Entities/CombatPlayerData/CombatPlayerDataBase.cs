using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerDataBase : ICombatPlayerRefs
{
    public int Id { get; protected set; }

    public int CombatPlayerId { get; protected set; }

    public void SetCombatPlayerId(int combatPlayerId)
    {
        CombatPlayerId = combatPlayerId;
    }
}
