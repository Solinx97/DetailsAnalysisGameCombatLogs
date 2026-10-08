using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;
using System.Net.NetworkInformation;

namespace Chat.Application.Tests.Factory;

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

    public static GroupChatMessageDto CreateDto(
        string? username = null,
        string? message = null,
        int? chatId = null,
        GroupChatUserId? groupChatUserId = null,
        MessageStatus? status = null,
        MessageType? type = null,
        MessageMarkedType? markedType = null
    )
    {
        var entity = new GroupChatMessageDto
        {
            Id = Guid.NewGuid(),
            Username = username ?? "check",
            Message = message ?? "test message",
            GroupChatId = chatId ?? 1,
            GroupChatUserId = groupChatUserId ?? "uid-1"
        };

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

    public static GroupChatMessageDto[] CreateDtoCollection(
        int size = 3
    )
    {
        var collection = new GroupChatMessageDto[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new GroupChatMessageDto
            {
                Id = Guid.NewGuid(),
                Username = $"check-{i}",
                Message = $"test message {i}",
                GroupChatId = 1 + i,
                GroupChatUserId = $"uid-1-{i}",
                Status = MessageStatus.Sent,
                Type = MessageType.Default,
                MarkedType = MessageMarkedType.None
            };
        }

        return collection;
    }

    public static Domain.DTOs.GroupChatMessageDto[] CreateDomainDtoCollection(
        int size = 3
    )
    {
        var collection = new Domain.DTOs.GroupChatMessageDto[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new Domain.DTOs.GroupChatMessageDto(
                Id: 1 + i,
                Username: $"check-{i}",
                Message: $"test message {i}",
                Time: DateTimeOffset.Now,
                Status: MessageStatus.Sent,
                Type: MessageType.Default,
                MarkedType: MessageMarkedType.None,
                IsEdited: false,
                GroupChatId: 1 + i,
                GroupChatUserId: $"uid-1-{i}",
                AppUserId: $"uid-2-{i}"
                );
        }

        return collection;
    }
}
