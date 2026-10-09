using Chat.Domain.Entities;
using Chat.Domain.Enums.GroupChatRules;
using Chat.Domain.Exceptions;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Aggregates;

public class GroupChat : IRepositoryEntity<GroupChatId>
{
    private readonly List<GroupChatMessage> _messages = [];
    private readonly List<GroupChatUser> _users = [];

    public const int NAME_MAX_LENGTH = 128;

    private GroupChat() { }

    private GroupChat(string name, UserId ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

    public GroupChatId Id { get; private set; }

    public string Name { get; private set; }

    public UserId OwnerId { get; private set; }

    public GroupChatRules? Rules { get; private set; }

    public IReadOnlyCollection<GroupChatMessage> Messages => _messages.AsReadOnly();

    public IReadOnlyCollection<GroupChatUser> Users => _users.AsReadOnly();

    public static GroupChat Create(string name, string ownerUsername, UserId ownerId,
        int invitePeople, int removePeople, int pinMessage, int announcements)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, NAME_MAX_LENGTH, nameof(name));

        var chat = new GroupChat(name, ownerId);
        chat.AddUser(ownerUsername, ownerId);
        chat.AddRules((InvitePeopleRestrictions)invitePeople, (RemovePeopleRestrictions)removePeople, (PinMessageRestrictions)pinMessage, (AnnouncementsRestrictions)announcements);

        return chat;
    }

    public GroupChatUser AddUser(string username, UserId appUserId)
    {
        var user = GroupChatUser.Create(username, appUserId, 0);
        _users.Add(user);

        return user;
    }

    public GroupChatUser RemoveUser(GroupChatUserId userId)
    {
        var user = _users
            .FirstOrDefault(x => x.Id == userId)
                ?? throw new GroupChatUserNotFoundException(userId);

        _users.Remove(user);

        return user;
    }

    public void AddRules(InvitePeopleRestrictions invitePeople = InvitePeopleRestrictions.Anyone,
        RemovePeopleRestrictions removePeople = RemovePeopleRestrictions.Anyone,
        PinMessageRestrictions pinMessage = PinMessageRestrictions.Anyone,
        AnnouncementsRestrictions announcements = AnnouncementsRestrictions.Anyone)
    {
        Rules = GroupChatRules.Create(invitePeople, removePeople, pinMessage, announcements);
    }

    public void EnsureUserIsMember(GroupChatUser user)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(user.GroupChatId, Id, nameof(user));
    }

    public void UpdateName(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName, nameof(newName));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newName.Length, NAME_MAX_LENGTH, nameof(newName));

        if (!string.Equals(Name, newName, StringComparison.Ordinal))
        {
            Name = newName;
        }
    }

    public void EditMessage(GroupChatMessageId messageId, string newMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newMessage, nameof(newMessage));

        var groupChatMessage = Messages.SingleOrDefault(m => m.Id == messageId);
        if (groupChatMessage != null)
        {
            groupChatMessage.EditMessage(newMessage);
        }
    }

    public void PassOwner(UserId ownerId)
    {
        ArgumentNullException.ThrowIfNull(ownerId, nameof(ownerId));

        if (!OwnerId.Equals(ownerId))
        {
            OwnerId = ownerId;
        }
    }

    public void RemoveRules()
    {
        Rules = null;
    }

    public void UpdateRules(int invitePeople,
        int removePeople,
        int pinMessage,
        int announcements)
    {
        ArgumentNullException.ThrowIfNull(Rules, nameof(Rules));

        Rules.Update((InvitePeopleRestrictions)invitePeople, (RemovePeopleRestrictions)removePeople, (PinMessageRestrictions)pinMessage, (AnnouncementsRestrictions)announcements);
    }

    public void RemoveMessage(Guid messageId)
    {
        var message = _messages
            .FirstOrDefault(x => x.Id.Equals(messageId))
                ?? throw new GroupChatMessageNotFoundException(messageId);

        _messages.Remove(message);
    }
}
