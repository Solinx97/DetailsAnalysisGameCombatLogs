//using Chat.Application.DTOs;
//using Chat.Domain.Aggregates;

//namespace Chat.Application.Tests.Factory;

//internal static class PersonalChatTestData
//{
//    public static PersonalChat Create(
//        string? initiatorId = null,
//        string? companionId = null,
//        int? initiatorUnreadMessages = null,
//        int? companionUnreadMessages = null
//    )
//    {
//        var entity = new PersonalChat(
//            initiatorId: initiatorId ?? "uid-1",
//            companionId: companionId ?? "uid-2",
//            initiatorUnreadMessages: initiatorUnreadMessages ?? 0,
//            companionUnreadMessages: companionUnreadMessages ?? 0
//        );

//        return entity;
//    }

//    public static PersonalChatDto CreateDto(
//        string? initiatorId = null,
//        string? companionId = null,
//        int? initiatorUnreadMessages = null,
//        int? companionUnreadMessages = null
//    )
//    {
//        var entity = new PersonalChatDto
//        {
//            InitiatorId = initiatorId ?? "uid-1",
//            CompanionId = companionId ?? "uid-2",
//            InitiatorUnreadMessages = initiatorUnreadMessages ?? 0,
//            CompanionUnreadMessages = companionUnreadMessages ?? 0
//        };

//        return entity;
//    }

//    public static PersonalChat[] CreateCollection(
//        int size = 3
//    )
//    {
//        var collection = new PersonalChat[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new PersonalChat(
//                initiatorId: $"uid-{i}",
//                companionId: $"uid-1-{i}",
//                initiatorUnreadMessages: 0 + i,
//                companionUnreadMessages: 0 + i
//            );
//        }

//        return collection;
//    }

//    public static PersonalChatDto[] CreateDtoCollection(
//        int size = 3
//    )
//    {
//        var collection = new PersonalChatDto[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new PersonalChatDto
//            {
//                InitiatorId = $"uid-{i}",
//                CompanionId = $"uid-1-{i}",
//                InitiatorUnreadMessages = 0 + i,
//                CompanionUnreadMessages = 0 + i
//            };
//        }

//        return collection;
//    }
//}
