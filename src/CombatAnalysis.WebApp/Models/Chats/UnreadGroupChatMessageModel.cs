namespace CombatAnalysis.WebApp.Models.Chats;

public class UnreadGroupChatMessageModel
{
    public int Id { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatMessageId { get; set; }
}
