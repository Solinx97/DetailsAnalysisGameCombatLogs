using MediatR;

namespace Chat.Application.Commands.PersonalChat.UpdateMessageText;

public record UpdateMessageTextCommand(
    Guid Id,
    string? Message
    ) : IRequest;
