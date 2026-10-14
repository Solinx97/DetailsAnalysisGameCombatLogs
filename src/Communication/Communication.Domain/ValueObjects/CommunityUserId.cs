namespace Communication.Domain.ValueObjects;

public record CommunityUserId
{
    public CommunityUserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(CommunityUserId id) => id.Value;

    public static implicit operator CommunityUserId(Guid value) => new(value);
}
