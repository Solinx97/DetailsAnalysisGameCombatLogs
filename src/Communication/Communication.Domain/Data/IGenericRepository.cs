namespace Communication.Domain.Data;

public interface IGenericRepository<TModel, TId>
    where TModel : class
    where TId : notnull
{
    Task AddAsync(TModel item, CancellationToken ct);

    Task<TModel> GetByIdAsync(TId id, CancellationToken ct);
}
