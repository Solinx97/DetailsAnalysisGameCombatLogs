namespace Chat.Application.DTOs;

public class GroupChatUserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public int UnreadMessages { get; set; }

    public Guid? LastReadMessageId { get; set; }

    public int GroupChatId { get; set; }

    public Guid AppUserId { get; set; }
}
