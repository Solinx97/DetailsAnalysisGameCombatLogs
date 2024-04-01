namespace CombatAnalysis.ChatDAL.Entities;

public class UnreadGroupChatMessage
{
    public int Id { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatMessageId { get; set; }
}
