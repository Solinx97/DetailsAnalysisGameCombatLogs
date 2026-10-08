using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
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

    public async Task<IEnumerable<PersonalChat>> GetByUserIdAsync(Guid userId, CancellationToken cancelationToken)
    {
        var chats = await _context.PersonalChat
            .AsNoTracking()
            .Where(x => x.InitiatorId.Equals(userId) || x.CompanionId.Equals(userId))
            .ToListAsync(cancelationToken);

        return chats;
    }

    public async Task<bool> IsExistAsync(Guid initiatorId, Guid companionId, CancellationToken cancelationToken)
    {
        var count = await _context.PersonalChat
                    .AsNoTracking()
                    .CountAsync(m => (m.InitiatorId.Equals(initiatorId) && m.CompanionId.Equals(companionId))
                        || (m.InitiatorId.Equals(companionId) && m.CompanionId.Equals(initiatorId)), cancelationToken);

        return count > 0;
    }
}
