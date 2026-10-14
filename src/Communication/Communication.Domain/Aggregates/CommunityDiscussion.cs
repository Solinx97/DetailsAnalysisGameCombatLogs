using Communication.Domain.Entities.Community;
using Communication.Domain.Exceptions;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Aggregates;

public class CommunityDiscussion
{
    public const int TITLE_MAX_LENGTH = 128;
    public const int CONTENT_MAX_LENGTH = 512;

    private readonly List<CommunityDiscussionComment> _communityDiscussionComments = [];

    private CommunityDiscussion()
    {
    }

    private CommunityDiscussion(string title, string content, CommunityId communityId,
        UserId appUserId, DateTimeOffset createdAt)
    {
        Title = title;
        Content = content;
        CommunityId = communityId;
        AppUserId = appUserId;
        CreatedAt = createdAt;
    }

    public CommunityDiscussionId Id { get; private set; }

    public string Title { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public CommunityId CommunityId { get; private set; }

    public UserId AppUserId { get; private set; }

    public Community Community { get; private set; }

    public IReadOnlyList<CommunityDiscussionComment> CommunityDiscussionComments => _communityDiscussionComments;

    public static CommunityDiscussion Create(string title, string content, CommunityId communityId, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityDiscussion(title, content, communityId, appUserId, createdAt);
    }

    public void Edit(string title, string content)
    {
        EditTile(title);
        EditContent(content);
    }

    public CommunityDiscussionComment AddComment(string content, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var comment = CommunityDiscussionComment.Create(content, appUserId);
        _communityDiscussionComments.Add(comment);

        return comment;
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

    private void EditTile(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        if (!string.Equals(Title, title, StringComparison.CurrentCultureIgnoreCase))
        {
            Title = title;
        }
    }

    private void EditContent(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        if (!string.Equals(Content, content, StringComparison.CurrentCultureIgnoreCase))
        {
            Content = content;
        }
    }
}
