using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Interfaces
{
    public interface ISPService<TModel, TIdType> : IService<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        Task<int> DeleteByProcedureAsync(int combatPlayerId);

        Task<int> CreateByProcedureAsync(TModel item);

        Task<IEnumerable<TModel>> GetByProcedureAsync(int id);
    }
}
