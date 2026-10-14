namespace Communication.Domain.ValueObjects;

public record CommunityPostLikeId
{
    public CommunityPostLikeId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(CommunityPostLikeId id) => id.Value;

    public static implicit operator CommunityPostLikeId(int value) => new(value);
}
