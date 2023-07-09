namespace CombatAnalysis.BL.DTO.Community;

public class CommunityPostDto
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public int CommentCount { get; set; }

    public string OwnerId { get; set; }

    public int CommunityId { get; set; }
}
