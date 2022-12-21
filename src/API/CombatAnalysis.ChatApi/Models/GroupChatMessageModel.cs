namespace CombatAnalysis.ChatApi.Models;

public class GroupChatMessageModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public string Time { get; set; }

    public int GroupChatId { get; set; }
}
