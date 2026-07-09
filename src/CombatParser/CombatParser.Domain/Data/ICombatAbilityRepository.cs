using CombatParser.Domain.Aggregates;
using CombatParser.Domain.DTOs;

namespace CombatParser.Domain.Data;

public interface ICombatAbilityRepository
{
    Task<IEnumerable<CombatAbility>> GetByAbilityTypeAsync(int combatPlayerId, int[] abilityTypes, CancellationToken cancellationToken);

    Task<Dictionary<string, int>> GetPotionsAsync(int combatLogId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayerPreAuraDto>> GetByPreAuraAsync(int combatId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayerPreAuraDto>> GetByPreAuraAsync(int combatId, int combatPlayerId, CancellationToken cancellationToken);
}
