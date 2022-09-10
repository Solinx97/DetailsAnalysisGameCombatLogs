using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.StoredProcedureDAL.Interfaces
{
    public interface IGenericRepository<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        Task<int> CreateAsync(string[] paramNames, object[] paramValues);

        Task<int> UpdateAsync(string[] paramNames, object[] paramValues);

        Task<int> DeleteAsync(TIdType id);

        Task<TModel> GetByIdAsync(TIdType id);

        Task<IEnumerable<TModel>> GetAllAsync();
    }
}
