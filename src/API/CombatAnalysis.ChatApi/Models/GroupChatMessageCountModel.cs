namespace CombatAnalysis.ChatApi.Models;

public class GroupChatMessageCountModel
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatId { get; set; }
}
