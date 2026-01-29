using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces.Filters;
using CombatParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Filters;

internal class DamageFilterRepository(CombatParserContextOne context) : IDamageFilterRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<List<CombatTarget>>> GetDamageByEachTargetAsync(int combatId, CancellationToken cancellationToken)
    {
        var damageByEachTarget = new List<List<CombatTarget>>();
        var targets = await GetTargetsAsync(combatId, cancellationToken);

        foreach (var item in targets)
        {
            var sum = await _context.Set<Combat>()
                       .Where(x => x.Id == combatId)
                       .Join(_context.Set<CombatPlayer>(),
                           x => x.Id,
                           u => u.CombatId,
                           (x, u) => new
                           {
                               u.Id,
                               u.Player.Username
                           })
                       .Join(_context.Set<DamageDone>(),
                           x => x.Id,
                           u => u.CombatPlayerId,
                           (x, u) => new
                           {
                               x.Username,
                               u.Target,
                               u.Value
                           })
                       .Where(x => x.Target == item)
                       .GroupBy(x => x.Username)
                       .Select(x => new CombatTarget(x.Key, item, x.Sum(y => y.Value), 0))
                       .OrderByDescending(x => x.Sum)
                       .ToListAsync(cancellationToken);

            damageByEachTarget.Add(sum);
        }

        return damageByEachTarget;
    }

    private async Task<List<string>> GetTargetsAsync(int combatId, CancellationToken cancellationToken)
    {
        var targets = await _context.Set<Combat>()
                .Where(x => x.Id == combatId)
                .Join(_context.Set<CombatPlayer>(),
                    x => x.Id,
                    u => u.CombatId,
                    (x, u) => new
                    {
                        u.Id,
                    })
                .Join(_context.Set<DamageDone>(),
                    x => x.Id,
                    u => u.CombatPlayerId,
                    (x, u) => new
                    {
                        u.Target
                    })
                .Distinct()
                .Select(x => x.Target)
                .ToListAsync(cancellationToken);

        return targets;
    }
}
