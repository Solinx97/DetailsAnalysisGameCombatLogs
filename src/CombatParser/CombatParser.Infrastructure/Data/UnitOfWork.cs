using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Persistent;

namespace CombatParser.Infrastructure.Data;

internal class UnitOfWork(CombatParserContextOne dbContext) : IUnitOfWork
{
    private readonly CombatParserContextOne _dbContext = dbContext;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
