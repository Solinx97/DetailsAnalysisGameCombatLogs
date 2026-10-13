using MediatR;

namespace Chat.Application.Commands.PersonalChat.UpdateMessageMark;

public record UpdateMessageMarkCommand(
    Guid Id,
    int Mark
    ) : IRequest;
