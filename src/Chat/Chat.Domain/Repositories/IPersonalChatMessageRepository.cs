using Chat.Domain.Entities;

namespace Chat.Domain.Repositories;

public interface IPersonalChatMessageRepository
{
    Task AddAsync(PersonalChatMessage message, CancellationToken cancelationToken);

    Task<IEnumerable<PersonalChatMessage>> GetAllAsync(CancellationToken cancelationToken);

    Task<PersonalChatMessage> GetByIdAsync(int id, CancellationToken cancelationToken);

    Task<IEnumerable<PersonalChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancelationToken);

    Task<int> CountAsync(int chatId, CancellationToken cancelationToken);

    Task DeleteAsync(Guid id, CancellationToken cancelationToken);
}
