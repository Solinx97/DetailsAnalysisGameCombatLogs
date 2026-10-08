using Chat.Domain.Enums;

namespace Chat.Application.DTOs;

public class PersonalChatMessageDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTimeOffset Time { get; set; }

    public MessageStatus Status { get; set; }

    public MessageType Type { get; set; }

    public MessageMarkedType MarkedType { get; set; }

    public int PersonalChatId { get; set; }

    public Guid AppUserId { get; set; }
}
