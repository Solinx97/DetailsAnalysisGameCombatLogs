namespace CombatAnalysis.EnhancedWebApp.Server.Models.Post;

public class UserPostLikeModel
{
    public int Id { get; set; }

    public int UserPostId { get; set; }

    public Guid AppUserId { get; set; }

    public int Status { get; set; }
}
