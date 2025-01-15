namespace CombatAnalysis.Core.Models.Chat;

public class GroupChatMessageCountModel
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string GroupChatUserId { get; set; }

    public int ChatId { get; set; }
}
