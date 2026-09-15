using CombatParser.Domain.Data;
using CombatParser.Domain.Data.Filters;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.Chart;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Filters;

internal class ChartRepository<TModel>(CombatParserContextOne context) : IChartRepository<TModel>
    where TModel : class, ICombatUnitRefs, IGeneralEntity
{
    const int INTERVAL = 10;
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<ChartGeneric>> GetUnitChartAsync(string unitId, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .Select(x => new
            {
                x.Time,
                x.Value
            })
            .ToListAsync(cancellationToken);

        var chart = values
            .GroupBy(x => (int)x.Time.TotalSeconds / INTERVAL)
            .Select(g => new ChartGeneric
            (
                g.Sum(x => x.Value),
                TimeSpan.FromSeconds(g.Key * INTERVAL)
            ))
            .OrderBy(x => x.Time)
            .ToList();

        return chart;
    }

    public async Task<Dictionary<string, ChartGeneric[]>> GetChartAsync(int combatId, CancellationToken cancellationToken)
    {
        var values = await _context.Set<CombatPlayer>()
            .AsNoTracking()
            .Where(x => x.CombatId == combatId)
            .Join(
                _context.Set<TModel>(),
                player => player.UnitId,
                resource => resource.UnitId,
                (player, resource) => new
                {
                    player.Player.Username,
                    resource.Value,
                    resource.Time
                })
            .ToListAsync(cancellationToken);

        var allCharts = values
            .GroupBy(x => x.Username)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(x => (int)x.Time.TotalSeconds / INTERVAL)
                    .Select(bucket => new ChartGeneric(
                        bucket.Sum(x => x.Value),
                        TimeSpan.FromSeconds(bucket.Key * INTERVAL)))
                    .OrderBy(x => x.Time)
                    .ToArray());

        return allCharts;
    }
}
