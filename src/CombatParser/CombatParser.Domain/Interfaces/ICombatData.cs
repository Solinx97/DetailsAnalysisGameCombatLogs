namespace CombatParser.Domain.Interfaces;

public interface ICombatData
{
    int CombatId { get; }

    void SetCombatId(int combatId);
}
