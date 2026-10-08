using Chat.Domain.Consts;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Outbox.Events;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatMessageRepository(ChatContext context) : IGroupChatMessageRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(GroupChatMessage message, CancellationToken cancelationToken)
    {
        await _context.GroupChatMessage
                     .AddAsync(message, cancelationToken);

        var @event = new GroupChatMessageCreatedEvent(Guid.NewGuid(), message.Id, message.GroupChatId, message.GroupChatUserId, message.Message);
        var outbox = new OutboxMessage
        {
            Id = @event.EventId,
            Topic = KafkaTopics.PERSONAL_CHAT_MESSAGE,
            Key = message.GroupChatId.ToString(),
            Payload = JsonSerializer.Serialize(@event)
        };

        await _context.OutboxMessages
                    .AddAsync(outbox, cancelationToken);
    }

    public async Task<IEnumerable<GroupChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancelationToken)
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

    public async Task<IEnumerable<GroupChatMessage>> GetAllAsync(CancellationToken cancelationToken)
    {
        var collection = await _context.GroupChatMessage
            .AsNoTracking()
            .ToListAsync(cancelationToken);

        return collection;
    }

    public async Task<GroupChatMessage> GetByIdAsync(int id, CancellationToken cancelationToken)
    {
        var entity = await _context.GroupChatMessage
            .SingleOrDefaultAsync(g => g.Id.Equals(id), cancelationToken)
                        ?? throw new EntityNotFoundException(typeof(GroupChatMessage), id);

        return entity;
    }

    public async Task<int> CountAsync(int chatId, CancellationToken cancelationToken)
    {
        var count = await _context.GroupChatMessage
                     .CountAsync(c => c.GroupChatId == chatId, cancelationToken);

        return count;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancelationToken)
    {
        var entity = await _context.GroupChatMessage
            .SingleOrDefaultAsync(g => g.Id.Equals(id), cancelationToken)
                    ?? throw new EntityNotFoundException(typeof(GroupChatMessage), id);

        _context.GroupChatMessage.Remove(entity);
    }
}
