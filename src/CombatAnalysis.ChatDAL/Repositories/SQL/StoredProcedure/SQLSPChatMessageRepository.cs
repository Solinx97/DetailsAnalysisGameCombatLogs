using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Repositories.SQL.StoredProcedure;

public class SQLSPChatMessageRepository<TModel, TIdType> : SQLRepository<TModel, TIdType>, IChatMessageRepository<TModel, TIdType>
    where TModel : BaseChatMessage
    where TIdType : notnull
{
    private readonly ChatSQLContext _context;

    public SQLSPChatMessageRepository(ChatSQLContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TModel>> GetByChatIdAsyn(int chatId, int pageSize)
    {
        var chatIdParam = new SqlParameter("ChatId", chatId);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSqlRaw($"Get{typeof(TModel).Name}ByChatIdPagination @chatId, @pageSize",
                                            chatIdParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<int> CountByChatIdAsync(int chatId)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(cl => cl.ChatId == chatId);

        return count;
    }
}
