namespace CombatAnalysis.Core.Models.Chat;

public class GroupChatModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string LastMessage { get; set; }

    public string AppUserId { get; set; }
}