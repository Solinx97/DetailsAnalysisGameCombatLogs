using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class PersonalChatMessageRepository(ChatContext context) : IPersonalChatMessageRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(PersonalChatMessage message, CancellationToken cancelationToken)
    {
        await _context.PersonalChatMessage
                     .AddAsync(message, cancelationToken);
    }

    public async Task<IEnumerable<PersonalChatMessage>> GetByChatIdAsync(PersonalChatId chatId, int page, int pageSize, CancellationToken cancelationToken)
    {
        var messages = await _context.PersonalChatMessage
                    .AsNoTracking()
                    .Where(m => m.PersonalChatId == chatId)
                    .OrderBy(m => m.Time)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancelationToken);

        return messages;
    }

    public async Task<IEnumerable<PersonalChatMessage>> GetAllAsync(CancellationToken cancelationToken)
    {
        var collection = await _context.PersonalChatMessage
            .AsNoTracking()
            .ToListAsync(cancelationToken);

        return collection;
    }

    public async Task<PersonalChatMessage> GetByIdAsync(PersonalChatMessageId id, CancellationToken cancelationToken)
    {
        var entity = await _context.PersonalChatMessage
            .SingleOrDefaultAsync(g => g.Id == id, cancelationToken)
                        ?? throw new EntityNotFoundException(typeof(PersonalChatMessage), id);

        return entity;
    }

    public async Task<int> CountAsync(PersonalChatId chatId, CancellationToken cancelationToken)
    {
        var count = await _context.PersonalChatMessage
                     .CountAsync(c => c.PersonalChatId == chatId, cancelationToken);

        return count;
    }

    public async Task DeleteAsync(PersonalChatMessageId id, CancellationToken cancelationToken)
    {
        var entity = await _context.PersonalChatMessage
            .SingleOrDefaultAsync(g => g.Id == id, cancelationToken)
                    ?? throw new EntityNotFoundException(typeof(PersonalChatMessage), id);

        _context.PersonalChatMessage.Remove(entity);
    }
}
