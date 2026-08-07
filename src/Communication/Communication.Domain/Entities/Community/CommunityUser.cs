namespace Communication.Domain.Entities.Community;

public class CommunityUser
{
    public const int USERNAME_MAX_LENGTH = 128;

    private CommunityUser()
    {
    }

    private CommunityUser(string username, int communityId, string appUserId)
    {
        Id = Guid.NewGuid().ToString();
        Username = username;
        CommunityId = communityId;
        AppUserId = appUserId;
    }

    public string Id { get; private set; }

    public string Username { get; private set; }

    public int CommunityId { get; private set; }

    public string AppUserId { get; private set; }

    public Aggregates.Community Community { get; private set; }

    public static CommunityUser Create(string username, int communityId, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new CommunityUser(username, communityId, appUserId);
    }
}
