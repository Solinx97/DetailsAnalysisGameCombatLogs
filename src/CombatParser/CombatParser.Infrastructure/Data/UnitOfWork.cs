using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Persistence;

namespace CombatParser.Infrastructure.Data;

internal class UnitOfWork(CombatParserContext dbContext) : IUnitOfWork
{
    private readonly CombatParserContext _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
