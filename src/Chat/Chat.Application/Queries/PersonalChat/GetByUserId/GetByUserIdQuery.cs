using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.PersonalChat.GetByUserId;

public record GetByUserIdQuery(
    Guid UserId
    ) : IRequest<IEnumerable<PersonalChatDto>>;
