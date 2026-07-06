using CombatParser.Domain.Data;
using CombatParser.Domain.Data.Filters;
using CombatParser.Domain.Entities.Chart;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Filters;

internal class ChartRepository<TModel>(CombatParserContextOne context) : IChartRepository<TModel>
    where TModel : class, ICombatPlayerRefs, IGeneralEntity
{
    const int INTERVAL = 10;
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<ChartGeneric>> GetChartAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
            .Where(x => x.CombatPlayerId == combatPlayerId)
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
}
