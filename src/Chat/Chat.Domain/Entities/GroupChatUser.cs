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

    public void MarkAsRead(GroupChatMessageId? messageId)
    {
        ArgumentNullException.ThrowIfNull(messageId, nameof(messageId));

        if (LastReadMessageId == null
            || (LastReadMessageId != null && (LastReadMessageId.Value == messageId.Value || messageId.Value > LastReadMessageId.Value)))
        {
            LastReadMessageId = messageId;
        }
    }

    public void UpdateUnreadMessages(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0, nameof(count));

        if (UnreadMessages != count)
        {
            UnreadMessages = count;
        }
    }
}
