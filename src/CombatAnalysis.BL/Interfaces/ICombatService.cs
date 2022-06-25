using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Interfaces
{
    public interface ICombatService<TModel>
        where TModel : class
    {
        Task<int> CreateAsync(TModel item);

        Task<int> UpdateAsync(TModel item);

        Task<int> DeleteAsync(TModel item);

        Task<IEnumerable<TModel>> GetAllAsync();

        Task<IEnumerable<TModel>> FindAllAsync(int combatLogId);

        Task<TModel> GetByIdAsync(int id);
    }
}
