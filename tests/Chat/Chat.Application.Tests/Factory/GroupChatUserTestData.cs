using Chat.Application.DTOs;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Tests.Factory;

internal static class GroupChatUserTestData
{
    public static GroupChatUser Create(
        string? id = null,
        string? username = null,
        int? chatId = null,
        UserId? appUserId = null,
        int? unreadMessages = null
    )
    {
        var entity = GroupChatUser.Create(
            username: username ?? "check",
            appUserId: appUserId ?? Guid.NewGuid(),
            unreadMessages: unreadMessages ?? 0
        );

        return entity;
    }

    public static GroupChatUserDto CreateDto(
        string? id = null,
        string? username = null,
        int? unreadMessages = null,
        int? chatId = null,
        UserId? appUserId = null
    )
    {
        var entity = new GroupChatUserDto
        {
            Id = Guid.NewGuid(),
            Username = username ?? "check",
            UnreadMessages = unreadMessages ?? 0,
            LastReadMessageId = Guid.NewGuid(),
            GroupChatId = chatId ?? 1,
            AppUserId = appUserId ?? Guid.NewGuid()
        };

        return entity;
    }

    public static GroupChatUser[] CreateCollection(
        int size = 3
    )
    {
        var collection = new GroupChatUser[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = GroupChatUser.Create(
                username: $"check-{i}",
                appUserId: Guid.NewGuid(),
                unreadMessages: 0 + i
            );
        }

        return collection;
    }

    public static GroupChatUserDto[] CreateDtoCollection(
        int size = 3
    )
    {
        var collection = new GroupChatUserDto[size];
        for (var i = 0; i < size; i++)
        {
            collection[i] = new GroupChatUserDto
            {
                Id = Guid.NewGuid(),
                Username = $"check-{i}",
                UnreadMessages = 0 + i,
                LastReadMessageId = null,
                GroupChatId = 1 + i,
                AppUserId = Guid.NewGuid()
            };
        }

        return collection;
    }
}
