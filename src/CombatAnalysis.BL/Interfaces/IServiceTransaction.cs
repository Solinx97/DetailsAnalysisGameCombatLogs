using CombatAnalysis.BL.Interfaces.General;
using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.BL.Interfaces;

public interface IServiceTransaction<TModel> : IQueryService<TModel>
    where TModel : class
{
    Task<int> DeleteUseExistTransactionAsync(IDbContextTransaction transaction, int id);
}