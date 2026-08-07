namespace Communication.Domain.Entities.Community;

public class InviteToCommunity
{
    private InviteToCommunity()
    {
    }

    private InviteToCommunity(int id, int communityId, string toAppUserId, DateTimeOffset createdAt,
       string appUserId)
    {
        Id = id;
        CommunityId = communityId;
        ToAppUserId = toAppUserId;
        CreatedAt = createdAt;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public int CommunityId { get; private set; }

    public string ToAppUserId { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public string AppUserId { get; private set; }

    public Aggregates.Community Community { get; private set; }

    public static InviteToCommunity Create(int id, int communityId, string toAppUserId, DateTimeOffset createdAt,
       string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(toAppUserId, nameof(toAppUserId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new InviteToCommunity(id, communityId, toAppUserId, createdAt, appUserId);
    }
}
