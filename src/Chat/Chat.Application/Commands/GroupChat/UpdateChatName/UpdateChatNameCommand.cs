using MediatR;

namespace Chat.Application.Commands.GroupChat.UpdateChatName;

public record UpdateChatNameCommand(
    int Id,
    string Name
    ) : IRequest;
