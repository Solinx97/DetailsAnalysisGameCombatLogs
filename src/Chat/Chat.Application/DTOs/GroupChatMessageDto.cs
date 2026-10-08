using Chat.Domain.Enums;

namespace Chat.Application.DTOs;

public class GroupChatMessageDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTimeOffset Time { get; set; }

    public MessageStatus Status { get; set; }

    public MessageType Type { get; set; }

    public MessageMarkedType MarkedType { get; set; }

    public bool IsEdited { get; set; }

    public int GroupChatId { get; set; }

    public string GroupChatUserId { get; set; } = string.Empty;

    public string AppUserId { get; set; } = string.Empty;
}
