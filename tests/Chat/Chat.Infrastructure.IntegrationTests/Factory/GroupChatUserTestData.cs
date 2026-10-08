//using Chat.Domain.Entities;
//using Chat.Domain.ValueObjects;

//namespace Chat.Infrastructure.IntegrationTests.Factory;

//internal static class GroupChatUserTestData
//{
//    public static GroupChatUser Create(
//        string? id = null,
//        string? username = null,
//        int? chatId = null,
//        UserId? appUserId = null,
//        int? unreadMessages = null
//    )
//    {
//        var entity = new GroupChatUser(
//            id: id ?? "uid-1",
//            username: username ?? "check",
//            chatId: chatId ?? 1,
//            appUserId: appUserId ?? "uid-1-1",
//            unreadMessages: unreadMessages ?? 0
//        );

//        return entity;
//    }

//    public static GroupChatUser[] CreateCollection(
//        int size = 3
//    )
//    {
//        var collection = new GroupChatUser[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new GroupChatUser(
//                id: $"uid-{i}",
//                username: $"check-{i}",
//                chatId: 1 + i,
//                appUserId: $"uid-1-{i}",
//                unreadMessages: 0 + i
//            );
//        }

//        return collection;
//    }
//}
