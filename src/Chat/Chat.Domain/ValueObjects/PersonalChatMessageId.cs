namespace Chat.Domain.ValueObjects;

public record PersonalChatMessageId
{
    public PersonalChatMessageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(PersonalChatMessageId id) => id.Value;

    public static implicit operator PersonalChatMessageId(Guid value) => new(value);
}
