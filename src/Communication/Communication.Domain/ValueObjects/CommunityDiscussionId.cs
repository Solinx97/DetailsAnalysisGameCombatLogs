namespace Communication.Domain.ValueObjects;

public record CommunityDiscussionId
{
    public CommunityDiscussionId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(CommunityDiscussionId id) => id.Value;

    public static implicit operator CommunityDiscussionId(int value) => new(value);
}
