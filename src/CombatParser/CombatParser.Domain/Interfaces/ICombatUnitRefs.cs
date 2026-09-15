namespace CombatParser.Domain.Interfaces;

public interface ICombatUnitRefs
{
    string Id { get; }

    string UnitId { get; }

    void SetUnitId(string combatUnitId);
}
