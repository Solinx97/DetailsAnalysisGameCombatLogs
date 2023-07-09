namespace CombatAnalysis.ChatApi.Models.Community;

public class CommunityPostDislikeModel
{
    public int Id { get; set; }

    public int CommunityPostId { get; set; }

    public string OwnerId { get; set; }
}
