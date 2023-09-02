namespace CombatAnalysis.DAL.Entities.Chat;

public class PersonalChatMessage
{
    public int Id { get; set; }

    public string Message { get; set; }

    public TimeSpan Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public int PersonalChatId { get; set; }

    public string CustomerId { get; set; }
}
