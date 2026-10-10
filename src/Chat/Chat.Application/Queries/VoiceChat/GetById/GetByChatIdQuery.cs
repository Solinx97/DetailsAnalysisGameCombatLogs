using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.VoiceChat.GetById;

public record GetByChatIdQuery(
    int ChatId
    ) : IRequest<VoiceChatDto>;
