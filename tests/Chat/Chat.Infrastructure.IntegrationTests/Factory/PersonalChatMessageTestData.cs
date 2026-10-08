using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Infrastructure.IntegrationTests.Factory;

internal static class PersonalChatMessageTestData
{
    public static PersonalChatMessage Create(
        string? username = null,
        string? message = null,
        int? chatId = null,
        UserId? appUserId = null,
        MessageStatus? status = null,
        MessageType? type = null,
        MessageMarkedType? markedType = null
    )
    {
        var entity = PersonalChatMessage.Create(
            username: username ?? "check",
            message: message ?? "test message",
            chatId: chatId ?? 1,
            appUserId: appUserId ?? "uid-1"
        );

        return entity;
    }

    public static PersonalChatMessage[] CreateCollection(
        int size = 3
    )
    {
        var collection = new PersonalChatMessage[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = PersonalChatMessage.Create(
                username: $"chat-{i}",
                message: $"test message {i}",
                chatId: i + 1,
                appUserId: $"uid-{i}"
            );
        }

        return collection;
    }
}
