using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.GroupChat.GetMessagesByChatId;

public record GetMessagesByChatIdQuery(
    int ChatId,
    int Page,
    int PageSize
    ) : IRequest<IEnumerable<GroupChatMessageDto>>;
