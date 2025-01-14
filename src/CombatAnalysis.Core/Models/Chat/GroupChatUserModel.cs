namespace CombatAnalysis.Core.Models.Chat;

public class GroupChatUserModel
{
    public string Id { get; set; } = string.Empty;

    public string Username { get; set; }

    public string AppUserId { get; set; }

    public int ChatId { get; set; }
}
