namespace Communication.Domain.ValueObjects;

public record CommunityId
{
    public CommunityId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(CommunityId id) => id.Value;

    public static implicit operator CommunityId(int value) => new(value);
}
