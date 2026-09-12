using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.Base;

public class CombatUnitDataBase : ICombatUnitRefs
{
    public string Id { get; protected set; }

    public string CombatUnitId { get; protected set; }

    public void SetCombatUnitId(string combatUnitId)
    {
        CombatUnitId = combatUnitId;
    }
}
