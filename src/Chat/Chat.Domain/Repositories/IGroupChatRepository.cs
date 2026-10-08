using Chat.Domain.Aggregates;

namespace Chat.Domain.Repositories;

public interface IGroupChatRepository
{
    Task AddAsync(GroupChat chat, CancellationToken cancelationToken);
}