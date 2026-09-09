using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class UnitRepository<TModel>(CombatParserContextOne context) : IUnitRepository<TModel>
    where TModel : class, ICombatRefs, IUnitRef, ITime
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IDictionary<string, IEnumerable<TModel>>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
                    .AsNoTracking()
                    .Where(x => x.CombatId == combatId)
                    .GroupBy(x => x.CreatorGameId)
                    .ToDictionaryAsync(x => x.Key, x => x.OrderBy(y => y.Time).Select(y => y), cancellationToken);

        return data.Count != 0 ? data : [];
    }

    public async Task<IDictionary<string, List<UnitHealth>>> GetHealthByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await (
            from combatPlayer in _context.Set<CombatPlayer>().AsNoTracking()
            where combatPlayer.CombatId == combatId

            join damageDone in _context.Set<DamageDone>().AsNoTracking()
                on combatPlayer.Id equals damageDone.CombatPlayerId

            join unit in _context.Set<CombatUnit>().AsNoTracking()
                on new { CombatId = combatPlayer.CombatId, GameId = damageDone.TargetGameId }
                equals new { CombatId = unit.CombatId, GameId = unit.GameId }

            select new
            {
                CreatorGameId = unit.GameId,
                CurrentHealth = damageDone.TargetCurrentHealth,
                MaxHealth = unit.Health,
                Time = damageDone.Time
            }
        ).ToListAsync(cancellationToken);

        var unitHealths = data
            .Select(x => UnitHealth.Create(
                x.CreatorGameId,
                x.CurrentHealth,
                x.MaxHealth,
                x.Time,
                x.CurrentHealth == 0,
                combatId))
            .GroupBy(x => x.CreatorGameId)
            .ToDictionary(
                x => x.Key,
                x => x.OrderBy(y => y.Time).ToList());

        return unitHealths;
    }
}
