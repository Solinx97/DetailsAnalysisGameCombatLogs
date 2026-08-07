using Communication.Domain.Entities.Community;

namespace Communication.Domain.Aggregates;

public class CommunityDiscussion
{
    public const int TITLE_MAX_LENGTH = 128;

    private readonly List<CommunityDiscussionComment> _communityDiscussionComments = [];

    private CommunityDiscussion()
    {
    }

    private CommunityDiscussion(int id, string title, string content, DateTimeOffset createdAt, 
        int communityId, string appUserId)
    {
        Id = id;
        Title = title;
        Content = content;
        CreatedAt = createdAt;
        CommunityId = communityId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Title { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityId { get; private set; }

    public string AppUserId { get; private set; }

    public Community Community { get; private set; }

    public IReadOnlyList<CommunityDiscussionComment> CommunityDiscussionComments => _communityDiscussionComments;

    public static CommunityDiscussion Create(int id, string title, string content, DateTimeOffset createdAt,
        int communityId, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new CommunityDiscussion(id, title, content, createdAt, communityId, appUserId);
    }
}
