using MediatR;

namespace Chat.Application.Commands.PersonalChat.CreateMessage;

public record CreateMessageCommand(
    string Username,
    string Message,
    int ChatId,
    string AppUserId
    ) : IRequest;
