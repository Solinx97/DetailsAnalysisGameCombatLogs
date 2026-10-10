namespace Chat.Domain.ValueObjects;

public record VoiceChatParticipantId
{
    public VoiceChatParticipantId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(VoiceChatParticipantId id) => id.Value;


    public static implicit operator VoiceChatParticipantId(Guid value) => new(value);
}
