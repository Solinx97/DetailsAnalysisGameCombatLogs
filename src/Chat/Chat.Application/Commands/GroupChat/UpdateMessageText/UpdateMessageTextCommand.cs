using MediatR;

namespace Chat.Application.Commands.GroupChat.UpdateMessageText;

public record UpdateMessageTextCommand(
    Guid Id,
    string? Message
    ) : IRequest;
