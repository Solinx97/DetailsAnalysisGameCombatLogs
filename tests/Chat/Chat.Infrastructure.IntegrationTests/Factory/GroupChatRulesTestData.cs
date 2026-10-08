//using Chat.Domain.Entities;
//using Chat.Domain.Enums.GroupChatRules;

//namespace Chat.Infrastructure.IntegrationTests.Factory;

//internal static class GroupChatRulesTestData
//{
//    public static GroupChatRules Create(
//        int? chatId = null,
//        InvitePeopleRestrictions? invitePeople = null,
//        RemovePeopleRestrictions? removePeople = null,
//        PinMessageRestrictions? pinMessage = null,
//        AnnouncementsRestrictions? announcements = null
//    )
//    {
//        var entity = new GroupChatRules(
//            chatId: chatId ?? 1,
//            invitePeople: invitePeople ?? InvitePeopleRestrictions.Anyone,
//            removePeople: removePeople ?? RemovePeopleRestrictions.Anyone,
//            pinMessage: pinMessage ?? PinMessageRestrictions.Anyone,
//            announcements: announcements ?? AnnouncementsRestrictions.Anyone
//        );

//        return entity;
//    }

//    public static GroupChatRules[] CreateCollection(
//        int size = 3
//    )
//    {
//        var collection = new GroupChatRules[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new GroupChatRules(
//                chatId: i + 1,
//                invitePeople: InvitePeopleRestrictions.Anyone,
//                removePeople: RemovePeopleRestrictions.Anyone,
//                pinMessage: PinMessageRestrictions.Anyone,
//                announcements: AnnouncementsRestrictions.Anyone
//            );
//        }

//        return collection;
//    }
//}
