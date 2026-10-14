namespace CombatAnalysis.EnhancedWebApp.Server.Models.User;

public class FriendModel
{
    public int Id { get; set; }

    public Guid WhoFriendId { get; set; }

    public string WhoFriendUsername { get; set; } = string.Empty;

    public Guid ForWhomId { get; set; }

    public string ForWhomUsername { get; set; } = string.Empty;
}
