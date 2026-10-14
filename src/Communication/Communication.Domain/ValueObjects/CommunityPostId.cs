namespace Communication.Domain.ValueObjects;

public record CommunityPostId
{
    public CommunityPostId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(CommunityPostId id) => id.Value;

    public static implicit operator CommunityPostId(int value) => new(value);
}
