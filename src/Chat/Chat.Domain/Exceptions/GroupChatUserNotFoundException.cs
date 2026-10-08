using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class GroupChatUserNotFoundException(Guid userId) : DomainException($"Chat user with Id '{userId}' was not found.", ExceptionCode.NotFound)
{
    public Guid UserId { get; } = userId;
}
