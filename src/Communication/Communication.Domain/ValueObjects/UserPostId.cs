namespace Communication.Domain.ValueObjects;

public record UserPostId
{
    public UserPostId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(UserPostId id) => id.Value;

    public static implicit operator UserPostId(int value) => new(value);
}
