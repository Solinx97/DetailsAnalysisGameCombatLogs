namespace Communication.Domain.ValueObjects;

public record CommunityDiscussionCommentId
{
    public CommunityDiscussionCommentId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(CommunityDiscussionCommentId id) => id.Value;

    public static implicit operator CommunityDiscussionCommentId(int value) => new(value);
}
