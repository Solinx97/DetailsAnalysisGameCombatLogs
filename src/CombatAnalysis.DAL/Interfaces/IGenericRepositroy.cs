using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Interfaces
{
    public interface IGenericRepository<TModel>
        where TModel : class
    {
        Task<int> CreateAsync(TModel item);

        Task<int> UpdateAsync(TModel item);

        Task<int> DeleteAsync(TModel item);

        Task<TModel> GetByIdAsync(int id);

        Task<IEnumerable<TModel>> GetAllAsync();

        Task<IEnumerable<TModel>> GetByProcedureAsync(string procedureName, string[] paramNames, object[] paramValuee);

        Task<int> DeleteByProcedureAsync(string procedureName, string[] paramNames, object[] paramValuee);
    }
}
