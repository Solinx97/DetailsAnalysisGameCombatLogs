//using Chat.Application.DTOs;
//using Chat.Domain.Aggregates;
//using Chat.Domain.ValueObjects;

//namespace Chat.Application.Tests.Factory;

//internal static class VoiceChatTestData
//{
//    public static VoiceChat Create(
//        string? id = null,
//        UserId? appUserId = null
//    )
//    {
//        var entity = new VoiceChat(
//            id: id ?? "uid-1",
//            appUserId: appUserId ?? Guid.NewGuid()
//        );

//        return entity;
//    }

//    public static VoiceChatDto CreateDto(
//        string? id = null,
//        UserId? appUserId = null
//    )
//    {
//        var entity = new VoiceChatDto
//        {
//            Id = id ?? "uid-1",
//            AppUserId = appUserId ?? Guid.NewGuid()
//        };

//        return entity;
//    }

//    public static VoiceChat[] CreateCollection(
//        int size = 3, bool addRules = false
//    )
//    {
//        var collection = new VoiceChat[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new VoiceChat(
//                id: $"uid-{i}",
//                appUserId: Guid.NewGuid()
//            );
//        }

//        return collection;
//    }

//    public static VoiceChatDto[] CreateDtoCollection(
//        int size = 3
//    )
//    {
//        var collection = new VoiceChatDto[size];
//        for (var i = 0; i < size; i++)
//        {
//            collection[i] = new VoiceChatDto
//            {
//                Id = $"uid-{i}",
//                AppUserId = Guid.NewGuid(),
//            };
//        }

//        return collection;
//    }
//}
