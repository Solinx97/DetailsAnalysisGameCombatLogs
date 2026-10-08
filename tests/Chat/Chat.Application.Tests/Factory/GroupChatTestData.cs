//using Chat.Application.DTOs;
//using Chat.Domain.Aggregates;
//using Chat.Domain.ValueObjects;

//namespace Chat.Application.Tests.Factory;

//internal static class GroupChatTestData
//{
//    public static GroupChat Create(
//        int? id = null,
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

//        if (id.HasValue)
//        {
//            entity.SetPrivateId(id.Value);
//        }

//        return entity;
//    }

//    public static GroupChatDto CreateDto(
//        int? id = null,
//        string? name = null,
//        UserId? ownerId = null
//    )
//    {
//        var entity = new GroupChatDto
//        {
//            Id = id ?? 1,
//            Name = name ?? "chat",
//            OwnerId = ownerId ?? "uid-1"
//        };

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

//    public static GroupChatDto[] CreateDtoCollection(
//        int size = 3
//    )
//    {
//        var collection = new GroupChatDto[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new GroupChatDto
//            {
//                Id = 1 + i,
//                Name = $"chat-{i}",
//                OwnerId = $"uid-1-{i}"
//            };
//        }

//        return collection;
//    }

//    private static GroupChat SetPrivateId(this GroupChat entity, GroupChatId id)
//    {
//        entity
//            .GetType()
//            .GetProperty(nameof(GroupChat.Id))?
//            .SetValue(entity, id);

//        return entity;
//    }
//}
