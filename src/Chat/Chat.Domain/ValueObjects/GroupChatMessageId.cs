namespace Chat.Domain.ValueObjects;

public record GroupChatMessageId
{
    public GroupChatMessageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(GroupChatMessageId id) => id.Value;

    public static implicit operator GroupChatMessageId(Guid value) => new(value);
}
