using MediatR;

namespace Chat.Application.Commands.VoiceChat.CreateChat;

public record CreateChatCommand(
    int GroupChatId,
    Guid AppUserId
    ) : IRequest;
