using CombatAnalysis.ChatDAL.Data;
using CombatAnalysis.ChatDAL.Interfaces;
using CombatAnalysis.ChatDAL.Interfaces.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.ChatDAL.Repositories.SQL.StoredProcedure;

internal class SQLSPChatMessageRepository<TModel, TIdType> : SQLRepository<TModel, TIdType>, IChatMessageRepository<TModel, TIdType>
    where TModel : class, IChatEntity
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

    public async Task<IEnumerable<TModel>> GetMoreByChatIdAsyn(int chatId, int offset, int pageSize)
    {
        var chatIdParam = new SqlParameter("ChatId", chatId);
        var offsetParam = new SqlParameter("Offset", offset);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSqlRaw($"Get{typeof(TModel).Name}ByChatIdMore @chatId, @offset, @pageSize",
                                            chatIdParam, offsetParam, pageSizeParam)
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
