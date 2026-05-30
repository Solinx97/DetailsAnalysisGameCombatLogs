using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Data;

public interface ICombatAbilityRepository
{
    Task<IEnumerable<CombatAbility>> GetByAbilityTypeAsync(int combatPlayerId, int[] abilityTypes, CancellationToken cancellationToken);
}
