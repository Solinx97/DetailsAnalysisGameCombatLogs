using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Entities.Events;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Chat.Infrastructure.Repositories;

internal class PersonalChatMessageRepository(ChatContext context) : IPersonalChatMessageRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(PersonalChatMessage message, CancellationToken cancelationToken)
    {
        await _context.PersonalChatMessage
                     .AddAsync(message, cancelationToken);

        var @event = new PersonalChatMessageCreatedEvent(Guid.NewGuid(), message.Id, message.PersonalChatId, message.AppUserId, message.Message);
        var outbox = new OutboxMessage
        {
            Id = @event.EventId,
            Topic = KafkaTopics.PERSONAL_CHAT_MESSAGE,
            Key = message.PersonalChatId.Value.ToString(),
            Payload = JsonSerializer.Serialize(@event)
        };

        await _context.OutboxMessages
                    .AddAsync(outbox, cancelationToken);
    }

    public async Task<IEnumerable<PersonalChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancelationToken)
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

    public async Task<PersonalChatMessage> GetByIdAsync(int id, CancellationToken cancelationToken)
    {
        var entity = await _context.PersonalChatMessage
            .SingleOrDefaultAsync(g => g.Id.Equals(id), cancelationToken)
                        ?? throw new EntityNotFoundException(typeof(PersonalChatMessage), id);

        return entity;
    }

    public async Task<int> CountAsync(int chatId, CancellationToken cancelationToken)
    {
        var count = await _context.PersonalChatMessage
                     .CountAsync(c => c.PersonalChatId == chatId, cancelationToken);

        return count;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancelationToken)
    {
        var entity = await _context.PersonalChatMessage
            .SingleOrDefaultAsync(g => g.Id.Equals(id), cancelationToken)
                    ?? throw new EntityNotFoundException(typeof(PersonalChatMessage), id);

        _context.PersonalChatMessage.Remove(entity);
    }
}
