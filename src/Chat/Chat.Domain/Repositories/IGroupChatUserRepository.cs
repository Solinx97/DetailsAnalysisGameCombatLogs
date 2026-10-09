using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatUserRepository
{
    Task<GroupChatUser> FindChatUserAsync(UserId appUserId, GroupChatId chatId, CancellationToken cancelationToken);

    Task<IEnumerable<GroupChatUser>> FindChatUsersAsync(UserId appUserId, CancellationToken cancelationToken);

    Task<IEnumerable<GroupChatUser>> FindAllChatUsersAsync(GroupChatId chatId, CancellationToken cancelationToken);
}
