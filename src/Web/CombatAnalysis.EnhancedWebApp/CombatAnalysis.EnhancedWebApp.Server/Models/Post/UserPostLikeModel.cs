namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public class UserPostLikeModel
{
    public int Id { get; set; }

    public int UserPostId { get; set; }

    public string AppUserId { get; set; } = string.Empty;

    public int Status { get; set; }
}
