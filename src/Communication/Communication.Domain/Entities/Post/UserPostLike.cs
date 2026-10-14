using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Post;

public class UserPostLike
{
    private UserPostLike()
    {
    }

    private UserPostLike(UserId appUserId)
    {
        AppUserId = appUserId;
    }

    public UserPostLikeId Id { get; private set; }

    public UserPostId UserPostId { get; private set; }

    public UserId AppUserId { get; private set; }

    public UserPost UserPost { get; private set; }

    public static UserPostLike Create(UserId appUserId)
    {
        return new UserPostLike(appUserId);
    }
}
