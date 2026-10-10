using Chat.Application.DTOs;
using MediatR;

namespace Chat.Application.Queries.VoiceChat.GetAll;

public record GetAllQuery(
    ) : IRequest<IEnumerable<VoiceChatDto>>;
