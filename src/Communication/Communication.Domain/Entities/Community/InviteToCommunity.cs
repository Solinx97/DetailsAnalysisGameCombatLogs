using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Community;

public class InviteToCommunity
{
    private InviteToCommunity()
    {
    }

    private InviteToCommunity(UserId toAppUserId, UserId appUserId, DateTimeOffset createdAt)
    {
        ToAppUserId = toAppUserId;
        AppUserId = appUserId;
        CreatedAt = createdAt;
    }

    public InviteToCommunityId Id { get; private set; }

    public CommunityId CommunityId { get; private set; }

    public UserId ToAppUserId { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public UserId AppUserId { get; private set; }

    public Aggregates.Community Community { get; private set; }

    public static InviteToCommunity Create(UserId toAppUserId, UserId appUserId)
    {
        var createdAt = DateTimeOffset.UtcNow;
        return new InviteToCommunity(toAppUserId, appUserId, createdAt);
    }
}
