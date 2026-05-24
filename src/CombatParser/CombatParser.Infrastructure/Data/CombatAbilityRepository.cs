using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatAbilityRepository(CombatParserContextOne context) : ICombatAbilityRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatAbility>> GetByAbilityTypeAsync(int combatPlayerId, int abilityType, CancellationToken cancellationToken)
    {
        var abilities = await _context.Set<CombatAbility>()
            .AsNoTracking()
            .Where(x => x.AbilityType == abilityType)
            .Join(_context.Set<CombatPlayerAura>(),
                ability => ability.GameId,
                aura => aura.GameAuraId,
                (ability, aura) => new { ability, aura })
            .Where(x => x.aura.CombatPlayerId == combatPlayerId)
            .Select(x => x.ability)
            .ToListAsync(cancellationToken);

        return abilities;
    }
}
