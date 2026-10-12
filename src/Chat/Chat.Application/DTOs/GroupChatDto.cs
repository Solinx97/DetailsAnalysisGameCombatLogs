namespace Chat.Application.DTOs;

public class GroupChatDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Guid OwnerId { get; set; }
}
