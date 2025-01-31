namespace CombatAnalysis.ChatApi.Models;

public class GroupChatMessageCountModel
{
    public int Id { get; set; }

    public int Count { get; set; }

    public int ChatId { get; set; }

    public string GroupChatUserId { get; set; }
}
