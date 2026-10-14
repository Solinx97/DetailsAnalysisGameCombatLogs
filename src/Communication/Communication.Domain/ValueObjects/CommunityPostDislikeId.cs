namespace Communication.Domain.ValueObjects;

public record CommunityPostDislikeId
{
    public CommunityPostDislikeId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(CommunityPostDislikeId id) => id.Value;

    public static implicit operator CommunityPostDislikeId(int value) => new(value);
}
