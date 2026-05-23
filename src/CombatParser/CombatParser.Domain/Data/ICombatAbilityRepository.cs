using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Data;

public interface ICombatAbilityRepository
{
    Task<IEnumerable<CombatAbility>> GetByAbiityTypeAsync(int abilityType, CancellationToken cancellationToken);
}
