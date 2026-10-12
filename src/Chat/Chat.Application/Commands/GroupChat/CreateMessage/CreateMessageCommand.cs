using MediatR;

namespace Chat.Application.Commands.GroupChat.CreateMessage;

public record CreateMessageCommand(
    Guid Id,
    string Username,
    string Message,
    int ChatId,
    Guid GroupChatUserId
    ) : IRequest;
