namespace CombatAnalysis.BL.DTO.Chat;

public class PersonalChatMessageDto
{
    public int Id { get; set; }

    public string Message { get; set; }

    public TimeSpan Time { get; set; }

    public int Status { get; set; }

    public int PersonalChatId { get; set; }

    public string OwnerId { get; set; }
}
