using Chat.Domain.Entities;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GroupChatUserRepository(ChatContext context) : IGroupChatUserRepository
{
    private readonly ChatContext _context = context;

    public async Task<GroupChatUser> FindChatUserAsync(UserId appUserId, GroupChatId chatId, CancellationToken cancelationToken)
    {
        var user = await _context.GroupChatUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.AppUserId == appUserId && m.GroupChatId == chatId, cancelationToken)
                        ?? throw new GroupChatUserNotFoundException(appUserId);

        return user;
    }

    public async Task<IEnumerable<GroupChatUser>> FindChatUsersAsync(UserId appUserId, CancellationToken cancelationToken)
    {
        var users = await _context.GroupChatUser
                    .AsNoTracking()
                    .Where(m => m.AppUserId.Equals(appUserId))
                    .ToListAsync(cancelationToken);

        return users;
    }

    public async Task<IEnumerable<GroupChatUser>> FindAllChatUsersAsync(GroupChatId chatId, CancellationToken cancelationToken)
    {
        var users = await _context.GroupChatUser
                    .AsNoTracking()
                    .Where(m => m.GroupChatId == chatId)
                    .ToListAsync(cancelationToken);

        return users;
    }
}
