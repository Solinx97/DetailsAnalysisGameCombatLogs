using CombatParser.Domain.Entities.Chart;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Data.Filters;

public interface IChartRepository<TModel>
    where TModel : class, ICombatPlayerRefs, IGeneralEntity
{
    Task<IEnumerable<ChartGeneric>> GetChartAsync(int combatPlayerId, CancellationToken cancellationToken);
}
