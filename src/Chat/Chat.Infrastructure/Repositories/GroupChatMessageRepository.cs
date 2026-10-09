using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatMessageRepository(ChatContext context) : IGroupChatMessageRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(GroupChatMessage message, CancellationToken cancelationToken)
    {
        await _context.GroupChatMessage
                     .AddAsync(message, cancelationToken);
    }

    public async Task<IEnumerable<GroupChatMessage>> GetByChatIdAsync(GroupChatId chatId, int page, int pageSize, CancellationToken cancelationToken)
    {
        var messages = await _context.GroupChatMessage
                    .AsNoTracking()
                    .Where(m => m.GroupChatId == chatId)
                    .OrderBy(m => m.Time)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancelationToken);

        return messages;
    }

    public async Task<int> CountAsync(GroupChatId chatId, CancellationToken cancelationToken)
    {
        var count = await _context.GroupChatMessage
                     .CountAsync(c => c.GroupChatId == chatId, cancelationToken);

        return count;
    }
}
