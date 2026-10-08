using MediatR;

namespace Chat.Application.Commands.PersonalChat.DeleteChat;

public record DeleteChatCommand(
    int Id
    ) : IRequest;
