namespace CombatAnalysis.ChatDAL.Entities;

public class PersonalChatMessage : BaseChatMessage
{
    public int Id { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public override int ChatId { get; set; }

    public string AppUserId { get; set; }
}
