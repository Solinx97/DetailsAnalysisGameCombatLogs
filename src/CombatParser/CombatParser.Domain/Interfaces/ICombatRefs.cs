namespace CombatParser.Domain.Interfaces;

public interface ICombatRefs
{
    int CombatId { get; }

    void SetCombatId(int combatId);
}
