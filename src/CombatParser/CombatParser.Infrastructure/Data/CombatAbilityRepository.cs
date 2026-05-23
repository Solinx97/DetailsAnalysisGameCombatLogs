using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatAbilityRepository(CombatParserContextOne context) : ICombatAbilityRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatAbility>> GetByAbiityTypeAsync(int abilityType, CancellationToken cancellationToken)
    {
        var abilitites = await _context.Set<CombatAbility>()
            .AsNoTracking()
            .Where(x => x.AbilityType == abilityType)
            .ToListAsync(cancellationToken);

        return abilitites;
    }
}
