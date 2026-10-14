using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Post;

public class UserPostDislike
{
    private UserPostDislike()
    {
    }

    private UserPostDislike(UserId appUserId)
    {
        AppUserId = appUserId;
    }

    public UserPostDislikeId Id { get; private set; }

    public UserPostId UserPostId { get; private set; }

    public UserId AppUserId { get; private set; }

    public UserPost UserPost { get; private set; }

    public static UserPostDislike Create(UserId appUserId)
    {
        return new UserPostDislike(appUserId);
    }
}
