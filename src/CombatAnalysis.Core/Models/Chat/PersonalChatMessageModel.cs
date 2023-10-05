namespace CombatAnalysis.Core.Models.Chat;

public class PersonalChatMessageModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public string Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public int PersonalChatId { get; set; }

    public string CustomerId { get; set; }
}
