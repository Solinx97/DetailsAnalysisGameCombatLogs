using Chat.Domain.Aggregates;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatRepository
{
    Task AddAsync(GroupChat chat, CancellationToken cancelationToken);

    Task<GroupChat> GetWithUsersAsync(GroupChatId id, CancellationToken cancelationToken);
}