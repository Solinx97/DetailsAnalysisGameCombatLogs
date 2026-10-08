namespace Chat.Application.DTOs;

public class PersonalChatDto
{
    public int Id { get; set; }

    public Guid InitiatorId { get; set; }

    public int? InitiatorUnreadMessages { get; set; }

    public Guid CompanionId { get; set; }

    public int? CompanionUnreadMessages { get; set; }
}
