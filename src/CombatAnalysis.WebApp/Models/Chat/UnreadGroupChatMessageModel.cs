namespace CombatAnalysis.WebApp.Models.Chat;

public class UnreadGroupChatMessageModel
{
    public int Id { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatMessageId { get; set; }
}
