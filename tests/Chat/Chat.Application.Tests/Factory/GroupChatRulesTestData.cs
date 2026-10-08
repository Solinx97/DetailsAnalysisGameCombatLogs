//using Chat.Application.DTOs;
//using Chat.Domain.Entities;
//using Chat.Domain.Enums.GroupChatRules;

//namespace Chat.Application.Tests.Factory;

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

//    public static GroupChatRulesDto CreateDto(
//        int? id = null,
//        int? chatId = null,
//        InvitePeopleRestrictions? invitePeople = null,
//        RemovePeopleRestrictions? removePeople = null,
//        PinMessageRestrictions? pinMessage = null,
//        AnnouncementsRestrictions? announcements = null
//    )
//    {
//        var entity = new GroupChatRulesDto
//        {
//            Id = id ?? 1,
//            InvitePeople = invitePeople ?? InvitePeopleRestrictions.Anyone,
//            RemovePeople = removePeople ?? RemovePeopleRestrictions.Anyone,
//            PinMessage = pinMessage ?? PinMessageRestrictions.Anyone,
//            Announcements = announcements ?? AnnouncementsRestrictions.Anyone,
//            GroupChatId = chatId ?? 1,
//        };

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

//    public static GroupChatRulesDto[] CreateDtoCollection(
//        int size = 3
//    )
//    {
//        var collection = new GroupChatRulesDto[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new GroupChatRulesDto
//            {
//                Id = 1 + i,
//                InvitePeople = InvitePeopleRestrictions.Anyone,
//                RemovePeople = RemovePeopleRestrictions.Anyone,
//                PinMessage = PinMessageRestrictions.Anyone,
//                Announcements = AnnouncementsRestrictions.Anyone,
//                GroupChatId = 1 + i,
//            };
//        }

//        return collection;
//    }
//}
