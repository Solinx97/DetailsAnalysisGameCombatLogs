namespace CombatAnalysis.ChatBL.Interfaces;

public interface IChatMessageService<TModel, TIdType> : IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<IEnumerable<TModel>> GetByChatIdAsync(int chatId, int pageSize);

    Task<IEnumerable<TModel>> GetMoreByChatIdAsync(int chatId, int offset, int pageSize);

    Task<int> CountByChatIdAsync(int chatId);
}