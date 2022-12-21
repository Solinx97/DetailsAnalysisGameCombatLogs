namespace CombatAnalysis.BL.DTO.Chat;

public class GroupChatMessageDto
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public TimeSpan Time { get; set; }

    public int GroupChatId { get; set; }
}
