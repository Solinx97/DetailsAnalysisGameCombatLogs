using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.GroupChat.FindChatUser;

public record FindChatUserQuery(
    Guid AppUserId,
    int ChatId
    ) : IRequest<GroupChatUserDto>;
