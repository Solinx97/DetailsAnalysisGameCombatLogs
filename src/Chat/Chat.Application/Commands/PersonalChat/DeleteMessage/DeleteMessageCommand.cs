using MediatR;

namespace Chat.Application.Commands.PersonalChat.DeleteMessage;

public record DeleteMessageCommand(
    Guid Id,
    int ChatId
    ) : IRequest;
