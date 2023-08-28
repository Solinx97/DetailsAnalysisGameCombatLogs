namespace CombatAnalysis.WebApp.Models.Chats;

public class GroupChatMessageCountModel
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string UserId { get; set; }

    public int GroupChatId { get; set; }
}
