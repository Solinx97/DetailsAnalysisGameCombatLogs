using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Data;

public interface ICombatAbilityRepository
{
    Task<IEnumerable<CombatAbility>> GetByAbilityTypeAsync(int combatPlayerId, int[] abilityTypes, CancellationToken cancellationToken);

    Task<Dictionary<string, int>> GetPotionsAsync(int combatLogId, CancellationToken cancellationToken);

    Task<IEnumerable<PreAuraEnchanced>> GetByPreAuraAsync(int combatId, CancellationToken cancellationToken);

    Task<IEnumerable<PreAuraEnchanced>> GetByPreAuraAsync(int combatId, string unitId, CancellationToken cancellationToken);
}
