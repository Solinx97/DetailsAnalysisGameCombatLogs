namespace CombatAnalysis.ChatBL.Interfaces;

public interface IChatMessageService<TModel, TIdType> : IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByChatIdAsyn(int chatId, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}