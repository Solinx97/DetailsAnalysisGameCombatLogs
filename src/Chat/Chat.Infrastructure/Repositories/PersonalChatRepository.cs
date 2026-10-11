using Chat.Domain.Aggregates;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class PersonalChatRepository(ChatContext context) : IPersonalChatRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(PersonalChat chat, CancellationToken cancelationToken)
    {
        await _context.PersonalChat
                     .AddAsync(chat, cancelationToken);
    }

    public async Task<IEnumerable<PersonalChat>> GetByUserIdAsync(UserId userId, CancellationToken cancelationToken)
    {
        var chats = await _context.PersonalChat
            .AsNoTracking()
            .Where(x => x.InitiatorId == userId || x.CompanionId == userId)
            .ToListAsync(cancelationToken);

        return chats;
    }

    public async Task<bool> IsExistAsync(UserId initiatorId, UserId companionId, CancellationToken cancelationToken)
    {
        var count = await _context.PersonalChat
                    .AsNoTracking()
                    .CountAsync(m => (m.InitiatorId == initiatorId && m.CompanionId == companionId)
                        || (m.InitiatorId == companionId && m.CompanionId == initiatorId), cancelationToken);

        return count > 0;
    }

    public async Task<PersonalChat> GetWithMessagesAsync(PersonalChatId id, CancellationToken cancelationToken)
    {
        var entity = await _context.PersonalChat
            .Include(x => x.Messages)
            .FirstOrDefaultAsync(g => g.Id == id, cancelationToken)
                        ?? throw new EntityNotFoundException(typeof(PersonalChat), id);

        return entity;
    }
}
