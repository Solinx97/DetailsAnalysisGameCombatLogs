namespace CombatAnalysis.CommunicationBL.DTO.Post;

public class CommunityPostDto
{
    public int Id { get; set; }

    public string CommunityName { get; set; }

    public string Owner { get; set; }

    public string Content { get; set; }

    public int PostType { get; set; }

    public int PublicType { get; set; }

    public int Restrictions { get; set; }

    public string Tags { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public int CommentCount { get; set; }

    public int CommunityId { get; set; }

    public string AppUserId { get; set; }
}
