using MediatR;

namespace Chat.Application.Commands.GroupChat.AddUser;

public record AddUserCommand(
    string Username,
    int GroupChatId,
    Guid AppUserId,
    Guid WhoAddId
    ) : IRequest;
