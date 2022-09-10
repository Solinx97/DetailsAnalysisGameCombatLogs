using System.Threading.Tasks;

namespace CombatAnalysis.BL.Interfaces
{
    public interface ISPDeleteService<TType>
        where TType : notnull
    {
        Task<int> DeleteByProcedureAsync(TType combatPlayerId);
    }
}
