namespace CombatAnalysis.CommunicationBL.DTO.Post;

public class CommunityPostDislikeDto
{
    public int Id { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int CommunityPostId { get; set; }

    public int CommunityId { get; set; }

    public string AppUserId { get; set; }
}
