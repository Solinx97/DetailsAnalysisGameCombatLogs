using MediatR;

namespace Chat.Application.Commands.PersonalChat.CreateChat;

public record CreateChatCommand(
    Guid InitiatorId,
    Guid CompanionId
    ) : IRequest;
