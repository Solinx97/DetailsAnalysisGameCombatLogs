using Communication.Domain.Data;
using Communication.Domain.Enums;
using Communication.Domain.ReadModel;
using Communication.Domain.ValueObjects;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserFeedRepository(CommunicationContext context) : IUserFeedRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<int> CountNewPostsAsync(UserId appUserId, List<UserId> friendsIds, DateTimeOffset lastCheck, CancellationToken cancellationToken)
    {
        var userPosts = await _context.UserPost
            .AsNoTracking()
            .Where(x => (x.AppUserId == appUserId || friendsIds.Contains(x.AppUserId)) 
                            && x.CreatedAt > lastCheck)
            .CountAsync(cancellationToken);

        var communityPosts =
            from post in _context.CommunityPost
                                    .AsNoTracking()
            join member in _context.CommunityUser
                                    .AsNoTracking()
                on post.CommunityId equals member.CommunityId
            where member.AppUserId == appUserId
                    && post.CreatedAt > lastCheck
            select new
            {
                post.Id.Value
            };

        var communityPostsCount = await communityPosts
            .CountAsync(cancellationToken);

        return userPosts + communityPostsCount;
    }

    public async Task<(IEnumerable<UserFeedReadModel>, int)> GetUserFeedAsync(UserId appUserId, List<UserId> friendsIds, int page, int pageSize, CancellationToken cancellationToken)
    {
        var userPosts = await _context.UserPost
            .AsNoTracking()
            .Where(x => x.AppUserId == appUserId || friendsIds.Contains(x.AppUserId))
            .Select(x => new UserFeedReadModel
            (
                x.Id.Value,
                x.Content,
                x.PublicType,
                x.Tags,
                x.CreatedAt,
                x.AppUserId,

                x.UserPostLikes.Count(),
                x.UserPostDislikes.Count(),
                x.UserPostComments.Count(),

                x.UserPostLikes.Any(l => l.AppUserId == appUserId)
                    ? (int)PostReaction.Like
                    : x.UserPostDislikes.Any(d => d.AppUserId == appUserId)
                        ? (int)PostReaction.Dislike

                        : (int)PostReaction.None,

                (string?)null,
                0,
                0,
                0
            ))
            .ToListAsync(cancellationToken);

        var communityPosts =
            from post in _context.CommunityPost
                                    .AsNoTracking()
            join community in _context.Community
                                    .AsNoTracking()
                on post.CommunityId equals community.Id
            join member in _context.CommunityUser
                                    .AsNoTracking()
                on post.CommunityId equals member.CommunityId
            where member.AppUserId == appUserId
            select new UserFeedReadModel
            (
                post.Id.Value,
                post.Content,
                post.PublicType,
                post.Tags,
                post.CreatedAt,
                post.AppUserId,

                post.CommunityPostLikes.Count(),
                post.CommunityPostDislikes.Count(),
                post.CommunityPostComments.Count(),

                post.CommunityPostLikes.Any(l => l.AppUserId == appUserId)
                    ? (int)PostReaction.Like
                    : post.CommunityPostDislikes.Any(d => d.AppUserId == appUserId)
                        ? (int)PostReaction.Dislike
                        : (int)PostReaction.None,

                community.Name,
                post.PostType,
                post.Restrictions,
                post.CommunityId.Value
            );

        var communityPostsItmes = await communityPosts
            .ToListAsync(cancellationToken);

        var feed = userPosts
            .Concat(communityPostsItmes)
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return (feed, feed.Count());
    }
}
