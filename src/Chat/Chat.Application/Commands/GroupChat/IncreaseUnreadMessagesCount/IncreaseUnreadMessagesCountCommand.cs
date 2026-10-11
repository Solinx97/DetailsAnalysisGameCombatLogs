using MediatR;

namespace Chat.Application.Commands.GroupChat.IncreaseUnreadMessagesCount;

public record IncreaseUnreadMessagesCountCommand(
    int ChatId,
    Guid SenderId
    ) : IRequest;
