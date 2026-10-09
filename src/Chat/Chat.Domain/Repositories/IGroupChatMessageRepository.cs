using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatMessageRepository
{
    Task AddAsync(GroupChatMessage message, CancellationToken cancelationToken);

    Task<IEnumerable<GroupChatMessage>> GetByChatIdAsync(GroupChatId chatId, int page, int pageSize, CancellationToken cancelationToken);

    Task<int> CountAsync(GroupChatId chatId, CancellationToken cancelationToken);
}
