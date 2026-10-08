using Chat.Domain.Enums;

namespace Chat.Domain.Exceptions;

public class PersonalChatMessageNotFoundException(Guid messageId) : DomainException($"Perosnal chat message with Id '{messageId}' was not found.", ExceptionCode.NotFound)
{
    public Guid MessageId { get; } = messageId;
}
