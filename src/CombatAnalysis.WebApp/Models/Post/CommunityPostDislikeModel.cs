namespace CombatAnalysis.WebApp.Models.Post;

public class CommunityPostDislikeModel
{
    public int Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int CommunityPostId { get; set; }

    public int CommunityId { get; set; }
}
