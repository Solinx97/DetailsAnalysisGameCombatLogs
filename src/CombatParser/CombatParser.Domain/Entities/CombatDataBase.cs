using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class CombatDataBase : ICombatRefs
{
    public int CombatId { get; protected set; }

    public void SetCombatId(int combatId)
    {
        CombatId = combatId;
    }
}
