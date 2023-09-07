namespace CombatAnalysis.BL.DTO.Community;

public class CommunityDiscussionCommentDto
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public string CustomerId { get; set; }

    public int CommunityDiscussionId { get; set; }
}
