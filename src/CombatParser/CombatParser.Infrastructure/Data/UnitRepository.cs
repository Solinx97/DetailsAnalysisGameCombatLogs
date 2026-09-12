using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class UnitRepository(CombatParserContextOne context) : IUnitRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatUnit>> GetAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<CombatUnit>()
                    .AsNoTracking()
                    .Where(x => x.CombatId == combatId)
                    .ToListAsync(cancellationToken);

        return data.Count != 0 ? data : [];
    }

    public async Task<IEnumerable<UnitPosition>> GetPositionsAsync(string combatUnitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitPosition>()
                    .AsNoTracking()
                    .Where(x => x.CombatUnitId == combatUnitId)
                    .ToListAsync(cancellationToken);

        return data.Count != 0 ? data : [];
    }

    public async Task<IEnumerable<UnitCast>> GetCastsAsync(string combatUnitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitCast>()
                    .AsNoTracking()
                    .Where(x => x.CombatUnitId == combatUnitId)
                    .ToListAsync(cancellationToken);

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
                on new { CombatId = combatPlayer.CombatId, GameId = damageDone.Target.GameId }
                equals new { CombatId = unit.CombatId, GameId = unit.GameId }

            select new
            {
                CreatorGameId = unit.GameId,
                CurrentHealth = damageDone.Target.Health,
                MaxHealth = unit.Health,
                Time = damageDone.Time,
                CombatUnit = unit.Id
            }
        ).ToListAsync(cancellationToken);

        var unitHealths = data
            .Select(x => UnitHealth.Create(
                x.CreatorGameId,
                x.CurrentHealth,
                x.MaxHealth,
                x.Time,
                x.CurrentHealth == 0))
            .GroupBy(x => x.CreatorGameId)
            .ToDictionary(
                x => x.Key,
                x => x.OrderBy(y => y.Time).ToList());

        return unitHealths;
    }
}
