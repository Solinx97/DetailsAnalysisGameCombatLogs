namespace CombatAnalysis.BL.DTO.Chat;

public class GroupChatMessageCountDto
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string CustomerId { get; set; }

    public int GroupChatId { get; set; }
}
