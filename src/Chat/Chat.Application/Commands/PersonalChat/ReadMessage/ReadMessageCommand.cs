using MediatR;

namespace Chat.Application.Commands.PersonalChat.ReadMessage;

public record ReadMessageCommand(
    Guid Id,
    int ChatId,
    Guid ReaderId
    ) : IRequest;
