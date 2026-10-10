using Chat.Domain.Aggregates;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class VoiceChatParticipant : IRepositoryEntity<VoiceChatParticipantId>
{
    private VoiceChatParticipant() { }

    private VoiceChatParticipant(UserId appUserId, DateTimeOffset joinedAt, DateTimeOffset lastSeenAt)
    {
        Id = Guid.NewGuid();
        AppUserId = appUserId;
        JoinedAt = joinedAt;
        LastSeenAt = lastSeenAt;
    }

    public VoiceChatParticipantId Id { get; private set; }

    public UserId AppUserId { get; private set; }

    public DateTimeOffset JoinedAt { get; private set; }

    public DateTimeOffset LastSeenAt { get; private set; }

    public VoiceChatId VoiceChatId { get; private set; }

    public VoiceChat VoiceChat { get; private set; }

    public static VoiceChatParticipant Create(UserId appUserId)
    {
        var joinedAt = DateTimeOffset.UtcNow;
        var lastSeenAt = DateTimeOffset.UtcNow;
        return new VoiceChatParticipant(appUserId, joinedAt, lastSeenAt);
    }
}
