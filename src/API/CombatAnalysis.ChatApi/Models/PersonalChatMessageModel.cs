namespace CombatAnalysis.ChatApi.Models;

public class PersonalChatMessageModel
{
    public int Id { get; set; }

    public string Message { get; set; }

    public string Time { get; set; }

    public int Status { get; set; }

    public int PersonalChatId { get; set; }

    public string OwnerId { get; set; }
}
