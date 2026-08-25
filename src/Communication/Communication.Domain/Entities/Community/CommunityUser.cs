namespace Communication.Domain.Entities.Community;

public class CommunityUser
{
    private CommunityUser()
    {
    }

    private CommunityUser(string appUserId)
    {
        Id = Guid.NewGuid().ToString();
        AppUserId = appUserId;
    }

    public string Id { get; private set; }

    public int CommunityId { get; private set; }

    public string AppUserId { get; private set; }

    public Aggregates.Community Community { get; private set; }

    public static CommunityUser Create(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new CommunityUser(appUserId);
    }
}
