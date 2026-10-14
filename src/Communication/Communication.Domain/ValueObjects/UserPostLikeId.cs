namespace Communication.Domain.ValueObjects;

public record UserPostLikeId
{
    public UserPostLikeId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(UserPostLikeId id) => id.Value;

    public static implicit operator UserPostLikeId(int value) => new(value);
}
