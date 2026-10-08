using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.Enums;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Tests.Factory;

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
            appUserId: appUserId ?? Guid.NewGuid()
        );

        return entity;
    }

    public static PersonalChatMessageDto CreateDto(
        int? id = null,
        string? username = null,
        string? message = null,
        int? chatId = null,
        UserId? appUserId = null,
        MessageStatus? status = null,
        MessageType? type = null,
        MessageMarkedType? markedType = null
    )
    {
        var entity = new PersonalChatMessageDto
        {
            Id = Guid.NewGuid(),
            Username = username ?? "check",
            Message = message ?? "test message",
            Time = DateTimeOffset.UtcNow,
            PersonalChatId = chatId ?? 1,
            AppUserId = appUserId ?? Guid.NewGuid(),
            Status = status ?? MessageStatus.Sent,
            Type = type ?? MessageType.Default,
            MarkedType = markedType ?? MessageMarkedType.None,
        };

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
                appUserId: Guid.NewGuid()
            );
        }

        return collection;
    }

    public static PersonalChatMessageDto[] CreateDtoCollection(
        int size = 3
    )
    {
        var collection = new PersonalChatMessageDto[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new PersonalChatMessageDto
            {
                Id = Guid.NewGuid(),
                Username = $"check-{i}",
                Message = $"test message {i}",
                Time = DateTimeOffset.UtcNow,
                PersonalChatId = 1,
                AppUserId =  Guid.NewGuid(),
                Status = MessageStatus.Sent,
                Type = MessageType.Default,
                MarkedType = MessageMarkedType.None,
            };
        }

        return collection;
    }
}
