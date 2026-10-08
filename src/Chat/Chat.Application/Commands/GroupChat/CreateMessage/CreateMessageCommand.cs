using MediatR;

namespace Chat.Application.Commands.GroupChat.CreateMessage;

public record CreateMessageCommand(
    string Username,
    string Message,
    int ChatId,
    Guid GroupChatUserId
    ) : IRequest;
