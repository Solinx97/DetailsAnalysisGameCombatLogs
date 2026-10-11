using Chat.Domain.Aggregates;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class GroupChatUser : IRepositoryEntity<GroupChatUserId>
{
    public const int USERNAME_MAX_LENGTH = 64;

    private GroupChatUser() { }

    private GroupChatUser(string username, UserId appUserId, int unreadMessages = 0)
    {
        Id = Guid.NewGuid();
        Username = username;
        UnreadMessages = unreadMessages;
        AppUserId = appUserId;
    }

    public GroupChatUserId Id { get; private set; }

    public string Username { get; private set; }

    public int UnreadMessages { get; private set; }

    public GroupChatMessageId? LastReadMessageId { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public UserId AppUserId { get; private set; }

    public GroupChat GroupChat { get; private set; } = null!;

    public static GroupChatUser Create(string username, UserId appUserId, int unreadMessages)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(username.Length, USERNAME_MAX_LENGTH, nameof(username));

        return new GroupChatUser(username, appUserId, unreadMessages);
    }

    public void ReadUnreadMessages(GroupChatMessageId messageId, int countUnreadMessagesRead)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(countUnreadMessagesRead, nameof(countUnreadMessagesRead));

        LastReadMessageId = messageId;

        UnreadMessages = countUnreadMessagesRead > UnreadMessages
            ? 0
            : UnreadMessages - countUnreadMessagesRead;
    }

    public void IncrementUnreadMessages()
    {
        UnreadMessages++;
    }
}
