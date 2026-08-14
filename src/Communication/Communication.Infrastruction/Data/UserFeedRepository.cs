using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Domain.Enums;
using Communication.Domain.ReadModel;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserFeedRepository(CommunicationContext context) : IUserFeedRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<(IEnumerable<UserFeedReadModel>, int)> GetUserFeedAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
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

                LikeCount = x.UserPostLikes.Count(),
                DislikeCount = x.UserPostDislikes.Count(),
                CommentCount = x.UserPostComments.Count(),

                Reaction = x.UserPostLikes.Any(l => l.AppUserId == appUserId)
                    ? (int)PostReaction.Like
                    : x.UserPostDislikes.Any(d => d.AppUserId == appUserId)
                        ? (int)PostReaction.Dislike

                        : (int)PostReaction.None,
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

                LikeCount = post.CommunityPostLikes.Count(),
                DislikeCount = post.CommunityPostDislikes.Count(),
                CommentCount = post.CommunityPostComments.Count(),

                Reaction = post.CommunityPostLikes.Any(l => l.AppUserId == appUserId)
                    ? (int)PostReaction.Like
                    : post.CommunityPostDislikes.Any(d => d.AppUserId == appUserId)
                        ? (int)PostReaction.Dislike
                        : (int)PostReaction.None,

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
            .Select(x => new UserFeedReadModel(
                x.Id,
                x.Owner,
                x.Content,
                x.PublicType,
                x.Tags,
                x.CreatedAt,
                x.AppUserId,

                x.LikeCount,
                x.DislikeCount,
                x.CommentCount,
                x.Reaction,

                x.CommunityName,
                x.PostType,
                x.Restrictions,
                x.CommunityId))
            .ToListAsync(cancellationToken);

        var count = await userPosts
            .Concat(communityPosts)
            .Select(x => new UserFeedReadModel(
                x.Id,
                x.Owner,
                x.Content,
                x.PublicType,
                x.Tags,
                x.CreatedAt,
                x.AppUserId,

                x.LikeCount,
                x.DislikeCount,
                x.CommentCount,
                x.Reaction,

                x.CommunityName,
                x.PostType,
                x.Restrictions,
                x.CommunityId))
            .CountAsync(cancellationToken);

        return (feed, count);
    }
}
