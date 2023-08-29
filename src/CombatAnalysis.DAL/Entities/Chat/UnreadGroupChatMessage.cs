namespace CombatAnalysis.DAL.Entities.Chat;

public class UnreadGroupChatMessage
{
    public int Id { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatMessageId { get; set; }
}
