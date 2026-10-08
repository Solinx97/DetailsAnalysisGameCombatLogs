using Chat.Domain.Aggregates;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Persistence;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatRepository(ChatContext context) : IGroupChatRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(GroupChat chat, CancellationToken cancelationToken)
    {
        await _context.GroupChat
                     .AddAsync(chat, cancelationToken);
    }
}
