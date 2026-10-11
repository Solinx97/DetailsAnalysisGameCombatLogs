using Chat.Domain.Aggregates;
using Chat.Domain.Enums;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class PersonalChatMessage : IRepositoryEntity<PersonalChatMessageId>
{
    public const int USERNAME_MAX_LENGTH = 64;
    public const int MESSAGE_MAX_LENGTH = 256;

    private PersonalChatMessage() { }

    private PersonalChatMessage(string username, string message, PersonalChatId chatId, UserId appUserId, long sequence,
                    MessageStatus status, MessageType type, MessageMarkedType markedType, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        Username = username;
        Message = message;
        PersonalChatId = chatId;
        Status = status;
        Type = type;
        MarkedType = markedType;
        AppUserId = appUserId;
        CreatedAt = createdAt;
        Sequence = sequence;
    }

    public PersonalChatMessageId Id { get; private set; }

    public string Username { get; private set; }

    public string Message { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public MessageStatus Status { get; private set; }

    public MessageType Type { get; private set; }

    public MessageMarkedType MarkedType { get; private set; }

    public bool IsEdited { get; private set; }

    public long Sequence { get; private set; }

    public PersonalChatId PersonalChatId { get; private set; }

    public UserId AppUserId { get; private set; }

    public PersonalChat PersonalChat { get; private set; } = null!;

    public static PersonalChatMessage Create(string username, string message, PersonalChatId chatId, UserId appUserId, long lastMessageSequence,
        int status = (int)MessageStatus.Sending, int messageType = (int)MessageType.Default, int markedType = (int)MessageMarkedType.None)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(message.Length, MESSAGE_MAX_LENGTH, nameof(message));
        ArgumentOutOfRangeException.ThrowIfNegative(lastMessageSequence, nameof(lastMessageSequence));

        var createdAt = DateTimeOffset.UtcNow;
        return new PersonalChatMessage(username, message, chatId, appUserId, ++lastMessageSequence, (MessageStatus)status, (MessageType)messageType, (MessageMarkedType)markedType, createdAt);
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

    public void ReadMessage()
    {
        if (Status != MessageStatus.Read)
        {
            Status = MessageStatus.Read;
        }
    }
}
