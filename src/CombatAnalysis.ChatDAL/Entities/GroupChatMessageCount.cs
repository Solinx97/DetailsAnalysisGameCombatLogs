namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChatMessageCount
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatId { get; set; }
}