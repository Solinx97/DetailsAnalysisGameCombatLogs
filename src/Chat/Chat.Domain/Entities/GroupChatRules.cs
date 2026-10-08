using Chat.Domain.Enums.GroupChatRules;
using Chat.Domain.Interfaces;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Entities;

public class GroupChatRules : IRepositoryEntity<GroupChatRulesId>
{
    private GroupChatRules() { }

    private GroupChatRules(InvitePeopleRestrictions invitePeople = InvitePeopleRestrictions.Anyone, 
        RemovePeopleRestrictions removePeople = RemovePeopleRestrictions.Anyone,
        PinMessageRestrictions pinMessage = PinMessageRestrictions.Anyone,
        AnnouncementsRestrictions announcements = AnnouncementsRestrictions.Anyone)
    {
        InvitePeople = invitePeople;
        RemovePeople = removePeople;
        PinMessage = pinMessage;
        Announcements = announcements;
    }

    public GroupChatRulesId Id { get; private set; }

    public InvitePeopleRestrictions InvitePeople { get; private set; }

    public RemovePeopleRestrictions RemovePeople { get; private set; }

    public PinMessageRestrictions PinMessage { get; private set; }

    public AnnouncementsRestrictions Announcements { get; private set; }

    public GroupChatId GroupChatId { get; private set; }

    public static GroupChatRules Create(InvitePeopleRestrictions invitePeople,
        RemovePeopleRestrictions removePeople,
        PinMessageRestrictions pinMessage,
        AnnouncementsRestrictions announcements)
    {
        return new GroupChatRules(invitePeople, removePeople, pinMessage, announcements);
    }

    public void Update(InvitePeopleRestrictions invitePeople,
        RemovePeopleRestrictions removePeople,
        PinMessageRestrictions pinMessage,
        AnnouncementsRestrictions announcements)
    {
        if (InvitePeople != invitePeople)
        {
            InvitePeople = invitePeople;
        }

        if (RemovePeople != removePeople)
        {
            RemovePeople = removePeople;
        }

        if (PinMessage != pinMessage)
        {
            PinMessage = pinMessage;
        }

        if (Announcements != announcements)
        {
            Announcements = announcements;
        }
    }
}
