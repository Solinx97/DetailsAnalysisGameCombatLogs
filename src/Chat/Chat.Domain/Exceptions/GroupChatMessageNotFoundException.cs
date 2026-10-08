using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class GroupChatMessageNotFoundException(Guid messageId) : DomainException($"Group chat message with Id '{messageId}' was not found.", ExceptionCode.NotFound)
{
    public Guid MessageId { get; } = messageId;
}
