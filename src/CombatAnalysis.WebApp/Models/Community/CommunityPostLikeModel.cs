namespace CombatAnalysis.WebApp.Models.Community;

public class CommunityPostLikeModel
{
    public int Id { get; set; }

    public int CommunityPostId { get; set; }

    public string OwnerId { get; set; }
}
