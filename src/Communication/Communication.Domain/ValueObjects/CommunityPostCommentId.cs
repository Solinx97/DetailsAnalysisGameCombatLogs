namespace Communication.Domain.ValueObjects;

public record CommunityPostCommentId
{
    public CommunityPostCommentId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(CommunityPostCommentId id) => id.Value;

    public static implicit operator CommunityPostCommentId(int value) => new(value);
}
