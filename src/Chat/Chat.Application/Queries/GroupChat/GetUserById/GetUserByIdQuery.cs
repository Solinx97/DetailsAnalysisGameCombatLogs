using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetUserById;

public record GetUserByIdQuery(
    Guid Id
    ) : IRequest<GroupChatUserDto>;
