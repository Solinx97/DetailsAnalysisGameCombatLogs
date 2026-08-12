using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Domain.ReadModel;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserFeedRepository(CommunicationContext context) : IUserFeedRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<UserFeed>> GetUserFeedAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var userPosts = _context.Set<UserPost>()
            .AsNoTracking()
            .Where(x => x.AppUserId == appUserId)
            .Select(x => new
            {
                x.Id,
                x.Owner,
                x.Content,
                x.PublicType,
                x.Tags,
                x.CreatedAt,
                x.AppUserId,

                CommunityName = (string)null,
                PostType = 0,
                Restrictions = 0,
                CommunityId = 0
            });

        var communityPosts =
            from post in _context.Set<CommunityPost>().AsNoTracking()
            join member in _context.Set<CommunityUser>().AsNoTracking()
                on post.CommunityId equals member.CommunityId
            where member.AppUserId == appUserId
            select new
            {
                post.Id,
                post.Owner,
                post.Content,
                post.PublicType,
                post.Tags,
                post.CreatedAt,
                post.AppUserId,

                post.CommunityName,
                post.PostType,
                post.Restrictions,
                post.CommunityId
            };

        var feed = await userPosts
            .Concat(communityPosts)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UserFeed(
                x.Id,
                x.Owner,
                x.Content,
                x.PublicType,
                x.Tags,
                x.CreatedAt,
                x.AppUserId,
                x.CommunityName,
                x.PostType,
                x.Restrictions,
                x.CommunityId))
            .ToListAsync(cancellationToken);

        return feed;
    }
}
