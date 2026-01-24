namespace CombatParser.Domain.Data;

public interface IGenericRepository<TModel, TId>
    where TModel : class
    where TId : notnull
{
    Task AddAsync(TModel item, CancellationToken ct = default);

    Task<IEnumerable<TModel>> GetAllAsync(CancellationToken ct = default);

    Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default);
}
