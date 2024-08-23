namespace CombatAnalysis.CommunicationBL.DTO.Post;

public class CommunityPostLikeDto
{
    public int Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int CommunityPostId { get; set; }

    public int CommunityId { get; set; }
}
