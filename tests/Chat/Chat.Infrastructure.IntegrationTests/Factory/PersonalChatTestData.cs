//using Chat.Domain.Aggregates;

//namespace Chat.Infrastructure.IntegrationTests.Factory;

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
//            initiatorId: initiatorId ?? "uid-22",
//            companionId: companionId ?? "uid-23",
//            initiatorUnreadMessages: initiatorUnreadMessages ?? 0,
//            companionUnreadMessages: companionUnreadMessages ?? 0
//        );

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
//}
