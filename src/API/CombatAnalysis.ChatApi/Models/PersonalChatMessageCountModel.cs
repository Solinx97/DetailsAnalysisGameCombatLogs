namespace CombatAnalysis.ChatApi.Models;

public class PersonalChatMessageCountModel
{
    public int Id { get; set; }

    public int Count { get; set; }

    public int ChatId { get; set; }

    public string AppUserId { get; set; }
}
