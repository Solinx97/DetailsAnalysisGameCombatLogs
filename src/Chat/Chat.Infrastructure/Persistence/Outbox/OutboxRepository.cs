using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces.Persistence;
using Chat.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Persistence.Outbox;

internal class OutboxRepository(ChatContext context, IMapper mapper) : IOutboxRepository
{
    private readonly ChatContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(Guid eventId, string eventType, string topic, string key, string payload, CancellationToken cancelationToken)
    {
        var outbox = new OutboxMessage
        {
            Id = eventId,
            EventType = eventType,
            Topic = topic,
            Key = key,
            Payload = payload
        };

        await _context.OutboxMessages
                    .AddAsync(outbox, cancelationToken);
    }

    public async Task<IEnumerable<OutboxMessageDto>> GetPendingAsync(int batchSize, CancellationToken cancellationToken)
    {
        var messages = await _context.OutboxMessages
            .AsNoTracking()
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        var map = _mapper.Map<IEnumerable<OutboxMessageDto>>(messages);
        return map;
    }

    public async Task MarkAsProcessedAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == eventId, cancellationToken)
                ?? throw new EntityNotFoundException(typeof(OutboxMessage), eventId);
        message.ProcessedAt = DateTime.UtcNow;
    }
}