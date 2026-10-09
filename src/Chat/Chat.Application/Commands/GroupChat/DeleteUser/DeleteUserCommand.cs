using MediatR;

namespace Chat.Application.Commands.GroupChat.DeleteUser;

public record DeleteUserCommand(
    Guid Id,
    int GroupChatId,
    Guid WhoDeleteId
    ) : IRequest;
