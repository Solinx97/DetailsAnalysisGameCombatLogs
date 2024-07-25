namespace CombatAnalysis.ChatBL.DTO;

public class PersonalChatMessageCountDto
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string AppUserId { get; set; }

    public int PersonalChatId { get; set; }
}