using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Community;

public class CommunityUser
{
    private CommunityUser()
    {
    }

    private CommunityUser(UserId appUserId)
    {
        Id = Guid.NewGuid();
        AppUserId = appUserId;
    }

    public CommunityUserId Id { get; private set; }

    public CommunityId CommunityId { get; private set; }

    public UserId AppUserId { get; private set; }

    public Aggregates.Community Community { get; private set; }

    public static CommunityUser Create(UserId appUserId)
    {
        return new CommunityUser(appUserId);
    }
}
