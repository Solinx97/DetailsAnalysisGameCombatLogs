namespace Communication.Domain.Entities.Community;

public class InviteToCommunity
{
    private InviteToCommunity()
    {
    }

    private InviteToCommunity(string toAppUserId, DateTimeOffset createdAt, string appUserId)
    {
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

    public static InviteToCommunity Create(string toAppUserId, DateTimeOffset createdAt, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(toAppUserId, nameof(toAppUserId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new InviteToCommunity(toAppUserId, createdAt, appUserId);
    }
}
