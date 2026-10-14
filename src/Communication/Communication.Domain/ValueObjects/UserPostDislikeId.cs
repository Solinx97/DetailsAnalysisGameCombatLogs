namespace Communication.Domain.ValueObjects;

public record UserPostDislikeId
{
    public UserPostDislikeId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(UserPostDislikeId id) => id.Value;

    public static implicit operator UserPostDislikeId(int value) => new(value);
}
