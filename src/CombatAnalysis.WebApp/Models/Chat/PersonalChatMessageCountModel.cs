namespace CombatAnalysis.WebApp.Models.Chat;

public class PersonalChatMessageCountModel
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string AppUserId { get; set; }

    public int ChatId { get; set; }
}
