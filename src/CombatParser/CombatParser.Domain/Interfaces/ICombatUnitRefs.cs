namespace CombatParser.Domain.Interfaces;

public interface ICombatUnitRefs
{
    string Id { get; }

    string CombatUnitId { get; }

    void SetCombatUnitId(string combatUnitId);
}
