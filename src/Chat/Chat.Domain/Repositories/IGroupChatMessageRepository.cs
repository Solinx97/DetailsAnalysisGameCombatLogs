using Chat.Domain.Entities;

namespace Chat.Domain.Repositories;

public interface IGroupChatMessageRepository
{
    Task AddAsync(GroupChatMessage message, CancellationToken cancelationToken);

    Task<IEnumerable<GroupChatMessage>> GetAllAsync(CancellationToken cancelationToken);

    Task<GroupChatMessage> GetByIdAsync(int id, CancellationToken cancelationToken);

    Task<IEnumerable<GroupChatMessage>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancelationToken);

    Task<int> CountAsync(int chatId, CancellationToken cancelationToken);

    Task DeleteAsync(Guid id, CancellationToken cancelationToken);
}
