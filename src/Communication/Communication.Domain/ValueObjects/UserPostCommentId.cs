namespace Communication.Domain.ValueObjects;

public record UserPostCommentId
{
    public UserPostCommentId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(UserPostCommentId id) => id.Value;

    public static implicit operator UserPostCommentId(int value) => new(value);
}
