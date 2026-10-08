using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.PersonalChat.GetMessagesByChatId;

public record GetMessagesByChatIdQuery(
    int ChatId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<PersonalChatMessageDto>>;
