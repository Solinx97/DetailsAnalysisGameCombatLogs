using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Interfaces
{
    public interface ISPGenericRepository<TModel> : IGenericRepository<TModel>
        where TModel : class
    {
        Task<IEnumerable<TModel>> ExecuteStoredProcedureUseModelAsync(string procedureName, string[] paramNames, object[] paramValuee);

        Task<int> ExecuteStoredProcedureAsync(string procedureName, string[] paramNames, object[] paramValuee);
    }
}