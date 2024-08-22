using CombatAnalysis.ChatDAL.Entities;

namespace CombatAnalysis.ChatDAL.Interfaces;

public interface IChatMessageRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByChatIdAsyn(int chatId, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}
