namespace CombatParser.Domain.Interfaces;

public interface ICombatPlayerData
{
    int CombatPlayerId { get; }

    void SetCombatPlayerId(int combatPlayerId);
}
