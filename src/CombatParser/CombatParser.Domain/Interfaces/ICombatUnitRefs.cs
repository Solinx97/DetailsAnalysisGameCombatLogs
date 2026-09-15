using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Interfaces;

public interface ICombatUnitRefs
{
    string Id { get; }

    Unit Unit { get; }

    string UnitId { get; }

    void SetUnitId(string combatUnitId);
}
