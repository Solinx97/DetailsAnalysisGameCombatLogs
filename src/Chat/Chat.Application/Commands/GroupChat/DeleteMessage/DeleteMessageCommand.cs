using MediatR;

namespace Chat.Application.Commands.GroupChat.DeleteMessage;

public record DeleteMessageCommand(
    Guid Id,
    int ChatId
    ) : IRequest;
