namespace Chat.Domain.ValueObjects;

public record VoiceChatId
{
    public VoiceChatId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(VoiceChatId id) => id.Value;


    public static implicit operator VoiceChatId(Guid value) => new(value);
}
