using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.Base;

public class CombatUnitDataBase : ICombatUnitRefs
{
    public string Id { get; protected set; }

    public Unit Unit { get; protected set; }

    public string UnitId { get; protected set; }

    public void SetUnitId(string unitId)
    {
        UnitId = unitId;
    }
}
