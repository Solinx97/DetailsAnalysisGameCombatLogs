namespace CombatAnalysis.WebApp.Models.Post;

public class UserPostLikeModel
{
    public int Id { get; set; }

    public int UserPostId { get; set; }

    public string AppUserId { get; set; }
}
