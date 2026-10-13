using MediatR;

namespace Chat.Application.Commands.GroupChat.UpdateMessageMark;

public record UpdateMessageMarkCommand(
    Guid Id,
    int Mark
    ) : IRequest;
