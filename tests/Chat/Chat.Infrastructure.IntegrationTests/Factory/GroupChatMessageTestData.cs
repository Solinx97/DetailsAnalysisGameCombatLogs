using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Infrastructure.IntegrationTests.Factory;

internal static class GroupChatMessageTestData
{
    public static GroupChatMessage Create(
        string? username = null,
        string? message = null,
        int? chatId = null,
        GroupChatUserId? groupChatUserId = null,
        MessageStatus? status = null,
        MessageType? type = null,
        MessageMarkedType? markedType = null
    )
    {
        var entity = GroupChatMessage.Create(
            username: username ?? "chat-1",
            message: message ?? "test message",
            chatId: chatId ?? 1,
            groupChatUserId: groupChatUserId ?? "uid-1"
        );

        return entity;
    }

    public static GroupChatMessage[] CreateCollection(
        int size = 3
    )
    {
        var collection = new GroupChatMessage[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = GroupChatMessage.Create(
                username: $"chat-{i}",
                message: $"test message {i}",
                chatId: i + 1,
                groupChatUserId: $"uid-{i}"
            );
        }

        return collection;
    }
}
