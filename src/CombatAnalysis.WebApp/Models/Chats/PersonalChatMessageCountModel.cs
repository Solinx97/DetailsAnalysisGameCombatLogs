namespace CombatAnalysis.WebApp.Models.Chats;

public class PersonalChatMessageCountModel
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string UserId { get; set; }

    public int PersonalChatId { get; set; }
}
