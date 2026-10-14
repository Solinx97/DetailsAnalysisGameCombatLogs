using Communication.Domain.Data;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;

namespace Communication.Infrastruction.Data;

internal class GenericRepository<TModel, TId>(CommunicationContext context) : IGenericRepository<TModel, TId>
    where TModel : class
    where TId : notnull
{
    private readonly CommunicationContext _context = context;

    public async Task AddAsync(TModel item, CancellationToken cancelationToken)
    {
        await _context.Set<TModel>().AddAsync(item, cancelationToken);
    }

    public async Task<TModel> GetByIdAsync(TId id, CancellationToken cancelationToken)
    {
        var entity = await _context.Set<TModel>()
            .FindAsync(id, cancelationToken)
            ?? throw new EntityNotFoundException(typeof(TModel), id);

        return entity;
    }
}
