using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IPersonalChatMessageRepository
{
    Task AddAsync(PersonalChatMessage message, CancellationToken cancelationToken);

    Task<IEnumerable<PersonalChatMessage>> GetAllAsync(CancellationToken cancelationToken);

    Task<PersonalChatMessage> GetByIdAsync(PersonalChatMessageId id, CancellationToken cancelationToken);

    Task<IEnumerable<PersonalChatMessage>> GetByChatIdAsync(PersonalChatId chatId, int page, int pageSize, CancellationToken cancelationToken);

    Task<int> CountAsync(PersonalChatId chatId, CancellationToken cancelationToken);

    Task DeleteAsync(PersonalChatMessageId id, CancellationToken cancelationToken);
}
