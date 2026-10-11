using MediatR;

namespace Chat.Application.Commands.GroupChat.ReadMessage;

public record ReadMessageCommand(
    Guid Id,
    int ChatId,
    Guid ReaderId
    ) : IRequest;
