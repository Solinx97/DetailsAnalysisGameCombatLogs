using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces.Filters;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Filters;

internal class DamageFilterRepository(CombatParserContextOne context) : IDamageFilterRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<List<CombatTarget>>> GetDamageByEachTargetAsync(int combatId, CancellationToken cancellationToken)
    {
        var damageByEachTarget = new List<List<CombatTarget>>();
        var targets = await GetTargetsAsync(combatId, cancellationToken);

        foreach (var target in targets)
        {
            var sum = await _context.Set<Combat>()
                            .AsNoTracking()
                            .Where(c => c.Id == combatId)
                            .SelectMany(c => c.CombatPlayers)
                            .SelectMany(cp => cp.DamageDones,
                                (cp, dd) => new
                                {
                                    cp.Player.Username,
                                    dd.Target,
                                    dd.Value,
                                    cp.CombatId
                                })
                            .GroupBy(x => new { x.Username, x.Target, x.CombatId })
                            .Where(c => c.Key.Target == target)
                            .Select(g => new
                            {
                                g.Key.Username,
                                g.Key.Target,
                                g.Key.CombatId,
                                Sum = g.Sum(x => x.Value)
                            })
                            .OrderByDescending(x => x.Sum)
                            .Select(x => new CombatTarget(
                                x.Username,
                                x.Target,
                                x.Sum,
                                x.CombatId))
                            .ToListAsync(cancellationToken);

            damageByEachTarget.Add(sum);
        }

        return damageByEachTarget;
    }

    private async Task<List<string>> GetTargetsAsync(int combatId, CancellationToken cancellationToken)
    {
        var targets = await _context.Set<Combat>()
                .AsNoTracking()
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
