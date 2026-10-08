using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IGroupChatUserRepository : IGenericRepository<GroupChatUser, GroupChatUserId>
{
    Task<IEnumerable<GroupChatUser>> FindAllAsync(int chatId);

    Task<IEnumerable<GroupChatUser>> FindAllByAppUserIdAsync(Guid appUserId);

    Task<GroupChatUser?> FindByAppUserIdAsync(int chatId, Guid appUserId);
}
