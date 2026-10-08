namespace Chat.Domain.ValueObjects;

public record GroupChatUserId
{
    public GroupChatUserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(GroupChatUserId id) => id.Value;

    public static implicit operator GroupChatUserId(Guid value) => new(value);
}
