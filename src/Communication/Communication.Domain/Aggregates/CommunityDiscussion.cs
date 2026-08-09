using Communication.Domain.Entities.Community;
using Communication.Domain.Exceptions;

namespace Communication.Domain.Aggregates;

public class CommunityDiscussion
{
    public const int TITLE_MAX_LENGTH = 128;

    private readonly List<CommunityDiscussionComment> _communityDiscussionComments = [];

    private CommunityDiscussion()
    {
    }

    private CommunityDiscussion(string title, string content, DateTimeOffset createdAt, int communityId,
        string appUserId)
    {
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

    public static CommunityDiscussion Create(string title, string content, int communityId, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityDiscussion(title, content, createdAt, communityId, appUserId);
    }

    public void EditTile(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        if (!string.Equals(Title, title, StringComparison.CurrentCultureIgnoreCase))
        {
            Title = title;
        }
    }

    public void AddComment(string content, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var comment = CommunityDiscussionComment.Create(content, appUserId);
        _communityDiscussionComments.Add(comment);
    }

    public void RemoveComment(int commentId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(commentId, nameof(commentId));

        var comment = _communityDiscussionComments
            .FirstOrDefault(x => x.Id == commentId)
                ?? throw new DomainException($"Community discussion comment not found with id {commentId}");

        _communityDiscussionComments.Remove(comment);
    }

    public void EditCommentContent(int discussionCommentId, string content)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(discussionCommentId, nameof(discussionCommentId));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var comment = _communityDiscussionComments
            .FirstOrDefault(x => x.Id == discussionCommentId)
                ?? throw new DomainException($"Community discussion comment not found with id {discussionCommentId}");

        comment.EditContent(content);
    }
}
