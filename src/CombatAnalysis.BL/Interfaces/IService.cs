using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Interfaces
{
    public interface IService<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        Task<int> CreateAsync(TModel item);

        Task<int> UpdateAsync(TModel item);

        Task<int> DeleteAsync(TModel item);

        Task<IEnumerable<TModel>> GetAllAsync();

        Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value);

        Task<TModel> GetByIdAsync(TIdType id);
    }
}
