using Chat.Domain.Entities;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class VoiceChat : IRepositoryEntity<VoiceChatId>
{
    private readonly List<VoiceChatParticipant> _participants = [];

    private VoiceChat() { }

    private VoiceChat(GroupChatId groupChatId, DateTimeOffset createdAt, DateTimeOffset lastAcrivityAt)
    {
        Id = Guid.NewGuid();
        GroupChatId = groupChatId;
        CreatedAt = createdAt;
        LastAcrivityAt = lastAcrivityAt;
    }

    public VoiceChatId Id { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset LastAcrivityAt { get; private set; }

    public IReadOnlyCollection<VoiceChatParticipant> Participants => _participants.AsReadOnly();

    public static VoiceChat Create(GroupChatId groupChatId, UserId appUserId)
    {
        var createdAt = DateTimeOffset.UtcNow;
        var lastAcrivityAt = DateTimeOffset.UtcNow;

        var chat = new VoiceChat(groupChatId, createdAt, lastAcrivityAt);
        chat.AddParticipant(appUserId);

        return chat;
    }

    public VoiceChatParticipant AddParticipant(UserId appUserId)
    {
        var participant = VoiceChatParticipant.Create(appUserId);
        _participants.Add(participant);

        return participant;
    }
}