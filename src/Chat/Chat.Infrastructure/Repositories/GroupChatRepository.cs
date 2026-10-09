using Chat.Domain.Aggregates;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatRepository(ChatContext context) : IGroupChatRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(GroupChat chat, CancellationToken cancelationToken)
    {
        await _context.GroupChat
                     .AddAsync(chat, cancelationToken);
    }

    public async Task<GroupChat> GetWithUsersAsync(GroupChatId id, CancellationToken cancelationToken)
    {
        var entity = await _context.GroupChat
            .Include(x => x.Users)
            .FirstOrDefaultAsync(g => g.Id == id, cancelationToken)
                        ?? throw new EntityNotFoundException(typeof(GroupChat), id);

        return entity;
    }
}
