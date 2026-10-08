using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatUserRepository(ChatContext context) : GenericRepository<GroupChatUser, GroupChatUserId>(context), IGroupChatUserRepository
{
    public async Task<IEnumerable<GroupChatUser>> FindAllAsync(int chatId)
    {
        var users = await _context.GroupChatUser
                            .AsNoTracking()
                            .Where(user => user.GroupChatId == chatId)
                            .ToListAsync();

        return users;
    }

    public async Task<IEnumerable<GroupChatUser>> FindAllByAppUserIdAsync(Guid appUserId)
    {
        var users = await _context.GroupChatUser
                            .AsNoTracking()
                            .Where(user => user.AppUserId.Equals(appUserId))
                            .ToListAsync();

        return users;
    }

    public async Task<GroupChatUser?> FindByAppUserIdAsync(int chatId, Guid appUserId)
    {
        var user = await _context.GroupChatUser
                            .AsNoTracking()
                            .FirstOrDefaultAsync(user => user.AppUserId.Equals(appUserId) && user.GroupChatId == chatId);

        return user;
    }
}
