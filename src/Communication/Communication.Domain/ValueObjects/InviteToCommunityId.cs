namespace Communication.Domain.ValueObjects;

public record InviteToCommunityId
{
    public InviteToCommunityId(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(InviteToCommunityId id) => id.Value;

    public static implicit operator InviteToCommunityId(int value) => new(value);
}
