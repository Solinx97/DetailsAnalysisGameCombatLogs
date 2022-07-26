using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Interfaces
{
    public interface IService<TModel>
        where TModel : class
    {
        Task<int> CreateAsync(TModel item);

        Task<int> UpdateAsync(TModel item);

        Task<int> DeleteAsync(TModel item);

        Task<int> DeleteByProcedureAsync(int combatPlayerId);

        Task<IEnumerable<TModel>> GetAllAsync();

        Task<IEnumerable<TModel>> GetByProcedureAsync(int id);

        Task<TModel> GetByIdAsync(int id);
    }
}
