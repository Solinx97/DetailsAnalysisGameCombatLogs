using Microsoft.EntityFrameworkCore.Storage;

namespace CombatAnalysis.BL.Interfaces;

public interface IServiceTransaction<TModel> : IService<TModel>
    where TModel : class
{
    Task<int> DeleteUseExistTransactionAsync(IDbContextTransaction transaction, int id);
}