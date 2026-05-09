namespace CombatParser.Domain.Interfaces;

public interface ICombatPlayerRefs
{
    int Id { get; }

    int CombatPlayerId { get; }

    void SetCombatPlayerId(int combatPlayerId);
}
