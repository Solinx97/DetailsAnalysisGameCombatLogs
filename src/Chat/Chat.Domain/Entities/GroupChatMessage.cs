using Chat.Domain.Aggregates;
using Chat.Domain.Enums;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class GroupChatMessage : IRepositoryEntity<GroupChatMessageId>, IChatEntity
{
    public const int USERNAME_MAX_LENGTH = 64;
    public const int MESSAGE_MAX_LENGTH = 256;

    private GroupChatMessage() { }

    private GroupChatMessage(string username, string message, GroupChatId chatId, GroupChatUserId groupChatUserId,
                    MessageStatus status, MessageType type, MessageMarkedType markedType, DateTimeOffset time)
    {
        Id = Guid.NewGuid();
        Username = username;
        Message = message;
        GroupChatId = chatId;
        GroupChatUserId = groupChatUserId;
        Status = status;
        Type = type;
        MarkedType = markedType;
        Time = time;
    }

    public GroupChatMessageId Id { get; private set; }

    public string Username { get; private set; }

    public string Message { get; private set; }

    public DateTimeOffset Time { get; private set; }

    public MessageStatus Status { get; private set; }

    public MessageType Type { get; private set; }

    public MessageMarkedType MarkedType { get; private set; }

    public bool IsEdited { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public GroupChatUserId GroupChatUserId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;

    public static GroupChatMessage Create(string username, string message, GroupChatId chatId, GroupChatUserId groupChatUserId,
            MessageStatus status = MessageStatus.Sending, MessageType messageType = MessageType.Default, MessageMarkedType markedType = MessageMarkedType.None)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(message.Length, MESSAGE_MAX_LENGTH, nameof(message));

        var time = DateTimeOffset.UtcNow;
        return new GroupChatMessage(username, message, chatId, groupChatUserId, status, messageType, markedType, time);
    }

    public void EditMessage(string? newMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newMessage, nameof(newMessage));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newMessage.Length, MESSAGE_MAX_LENGTH, nameof(newMessage));

        if (!string.Equals(Message, newMessage, StringComparison.Ordinal))
        {
            Message = newMessage;
            IsEdited = true;
        }
    }

    public void UpdateStatus(MessageStatus newStatus)
    {
        if (!Status.Equals(newStatus))
        {
            Status = newStatus;
        }
    }

    public void UpdateMarker(MessageMarkedType newMarkedType)
    {
        if (!MarkedType.Equals(newMarkedType))
        {
            MarkedType = newMarkedType;
        }
    }
}
