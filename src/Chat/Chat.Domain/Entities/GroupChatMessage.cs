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

    private GroupChatMessage(GroupChatMessageId id, string username, string message, GroupChatId chatId, GroupChatUserId groupChatUserId, long sequence,
                    MessageStatus status, MessageType type, MessageMarkedType markedType, DateTimeOffset createdAt)
    {
        Id = id;
        Username = username;
        Message = message;
        GroupChatId = chatId;
        Status = status;
        Type = type;
        MarkedType = markedType;
        GroupChatUserId = groupChatUserId;
        CreatedAt = createdAt;
        Sequence = sequence;
    }

    public GroupChatMessageId Id { get; private set; }

    public string Username { get; private set; }

    public string Message { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public MessageStatus Status { get; private set; }

    public MessageType Type { get; private set; }

    public MessageMarkedType MarkedType { get; private set; }

    public bool IsEdited { get; private set; }

    public long Sequence { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public GroupChatUserId GroupChatUserId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;

    public static GroupChatMessage Create(GroupChatMessageId id, string username, string message, GroupChatId chatId, GroupChatUserId groupChatUserId, long lastMessageSequence,
            int status = (int)MessageStatus.Sent, int messageType = (int)MessageType.Default, int markedType = (int)MessageMarkedType.None)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(message.Length, MESSAGE_MAX_LENGTH, nameof(message));

        var createdAt = DateTimeOffset.UtcNow;
        return new GroupChatMessage(id, username, message, chatId, groupChatUserId, ++lastMessageSequence, (MessageStatus)status, (MessageType)messageType, (MessageMarkedType)markedType, createdAt);
    }

    public void EditMessageText(string newMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newMessage, nameof(newMessage));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newMessage.Length, MESSAGE_MAX_LENGTH, nameof(newMessage));

        if (!string.Equals(Message, newMessage, StringComparison.Ordinal))
        {
            Message = newMessage;
            IsEdited = true;
        }
    }

    public void EditMark(int mark)
    {
        if (MarkedType != (MessageMarkedType)mark)
        {
            MarkedType = (MessageMarkedType)mark;
        }
    }

    public void ReadMessage()
    {
        if (Status != MessageStatus.Read)
        {
            Status = MessageStatus.Read;
        }
    }
}
