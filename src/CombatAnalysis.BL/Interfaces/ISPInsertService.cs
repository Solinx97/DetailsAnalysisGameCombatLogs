using System.Threading.Tasks;

namespace CombatAnalysis.BL.Interfaces
{
    public interface ISPInsertService<TModel>
        where TModel : class
    {
        Task<int> CreateByProcedureAsync(TModel item);
    }
}
