using Chat.Domain.Entities;
using Chat.Domain.Exceptions;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class PersonalChat : IRepositoryEntity<PersonalChatId>
{
    private readonly List<PersonalChatMessage> _messages = [];

    private PersonalChat() { }

    private PersonalChat(UserId initiatorId, UserId companionId, long lastMessageSequence,
        int initiatorUnreadMessages, int companionUnreadMessages)
    {
        InitiatorId = initiatorId;
        CompanionId = companionId;
        InitiatorUnreadMessages = initiatorUnreadMessages;
        CompanionUnreadMessages = companionUnreadMessages;
        LastMessageSequence = lastMessageSequence;
    }

    public PersonalChatId Id { get; private set; }

    public UserId InitiatorId { get; private set; }

    public int InitiatorUnreadMessages { get; private set; }

    public PersonalChatMessageId? LastInitiatorReadMessageId { get; private set; }

    public UserId CompanionId { get; private set; }

    public int CompanionUnreadMessages { get; private set; }

    public PersonalChatMessageId? LastCompanionReadMessageId { get; private set; }

    public long LastMessageSequence { get; private set; }

    public IReadOnlyCollection<PersonalChatMessage> Messages => _messages.AsReadOnly();

    public static PersonalChat Create(UserId initiatorId, UserId companionId,
        int initiatorUnreadMessages = 0, int companionUnreadMessages = 0)
    {
        return new PersonalChat(initiatorId, companionId, 0, initiatorUnreadMessages, companionUnreadMessages);
    }

    public void IncrementSequence()
    {
        LastMessageSequence++;
    }

    public void IncrementUnreadMessages(UserId messageOwnerId)
    {
        if (InitiatorId == messageOwnerId)
        {
            CompanionUnreadMessages++;
        }
        else if (CompanionId == messageOwnerId)
        {
            InitiatorUnreadMessages++;
        }
    }

    public void ReadMessage(PersonalChatMessageId messageId, UserId readerId)
    {
        var message = _messages
            .FirstOrDefault(x => x.Id == messageId)
                ?? throw new PersonalChatMessageNotFoundException(messageId);

        ReadUnreadMessages(message.Sequence, readerId);

        SetLastReadMessageId(messageId, readerId);
    }

    public void ReadUnreadMessages(long sequance, UserId readerId)
    {
        if (InitiatorId == readerId)
        {
            ReadInitiatorUnreadMessages(sequance, readerId);
        }
        else if (CompanionId == readerId)
        {
            ReadCompanionUnreadMessages(sequance, readerId);
        }
    }

    public void RemoveMessage(PersonalChatMessageId messageId)
    {
        var message = _messages
            .FirstOrDefault(x => x.Id.Equals(messageId))
                ?? throw new PersonalChatMessageNotFoundException(messageId);

        _messages.Remove(message);
    }

    private void SetLastReadMessageId(PersonalChatMessageId messageId, UserId readerId)
    {
        if (InitiatorId == readerId)
        {
            LastInitiatorReadMessageId = messageId;
        }
        else if(CompanionId == readerId)
        {
            LastCompanionReadMessageId = messageId;
        }
    }

    private void ReadInitiatorUnreadMessages(long sequance, UserId readerId)
    {
        long lastReadMessageSequence = 0;
        if (LastInitiatorReadMessageId != null)
        {
            var lastReadMessage = _messages
                .FirstOrDefault(x => x.Id == LastInitiatorReadMessageId)
                    ?? throw new PersonalChatMessageNotFoundException(LastInitiatorReadMessageId);
            lastReadMessageSequence = lastReadMessage.Sequence;
        }

        var messages = _messages
            .Where(x => x.Sequence > lastReadMessageSequence 
                        && x.Sequence <= sequance
                        && x.AppUserId != readerId)
            .ToList();

        foreach (var message in messages)
        {
            message.ReadMessage();
        }

        InitiatorUnreadMessages = messages.Count > InitiatorUnreadMessages
            ? 0
            : InitiatorUnreadMessages - messages.Count;
    }

    private void ReadCompanionUnreadMessages(long sequance, UserId readerId)
    {
        long lastReadMessageSequence = 0;
        if (LastCompanionReadMessageId != null)
        {
            var lastReadMessage = _messages
                .FirstOrDefault(x => x.Id == LastCompanionReadMessageId)
                    ?? throw new PersonalChatMessageNotFoundException(LastCompanionReadMessageId);
            lastReadMessageSequence = lastReadMessage.Sequence;
        }

        var messages = _messages
            .Where(x => x.Sequence > lastReadMessageSequence 
                        && x.Sequence <= sequance
                        && x.AppUserId != readerId)
            .ToList();

        foreach (var message in messages)
        {
            message.ReadMessage();
        }

        CompanionUnreadMessages = messages.Count > CompanionUnreadMessages 
            ? 0
            : CompanionUnreadMessages - messages.Count;
    }
}
