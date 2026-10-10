using MediatR;

namespace Chat.Application.Commands.VoiceChat.DeleteChat;

public record DeleteChatCommand(
    Guid Id
    ) : IRequest;
