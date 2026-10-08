//using Chat.Domain.Aggregates;
//using Chat.Domain.ValueObjects;

//namespace Chat.Infrastructure.IntegrationTests.Factory;

//internal static class GroupChatTestData
//{
//    public static GroupChat Create(
//        string? name = null,
//        UserId? ownerId = null,
//        bool addRules = false
//    )
//    {
//        var entity = new GroupChat(
//            name: name ?? "chat-1",
//            ownerId: ownerId ?? "uid-23"
//        );

//        if (addRules)
//        {
//            entity.AddRules(1);
//        }

//        return entity;
//    }

//    public static GroupChat[] CreateCollection(
//        int size = 3, bool addRules = false
//    )
//    {
//        var collection = new GroupChat[size];
//        for (var i = 0; i < size; i++)
//        {
//            var chat = new GroupChat(
//                name: $"chat-{i}",
//                ownerId: $"uid-{i}"
//            );

//            if (addRules)
//            {
//                chat.AddRules(i + 1);
//            }

//            collection[i] = chat;
//        }

//        return collection;
//    }
//}
