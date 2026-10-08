using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetById;

public record GetByIdQuery(
    int Id
    ) : IRequest<GroupChatDto>;
