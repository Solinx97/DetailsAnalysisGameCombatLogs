using Chat.Domain.Interfaces;

namespace Chat.Domain.Repositories;

public interface IGenericRepository<TModel, TId>
    where TModel : class, IRepositoryEntity<TId>
    where TId : notnull
{
    Task DeleteAsync(TId id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel> GetByIdAsync(TId id);
}
