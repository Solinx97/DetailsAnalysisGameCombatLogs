using MediatR;

namespace Chat.Application.Commands.GroupChat.DeleteChat;

public record DeleteChatCommand(
    int Id
    ) : IRequest;
