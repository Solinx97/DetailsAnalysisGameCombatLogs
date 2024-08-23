namespace CombatAnalysis.CommunicationDAL.Entities.Post;

public class CommunityPostComment
{
    public int Id { get; set; }

    public string Content { get; set; }

    public int CommentType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int CommunityPostId { get; set; }

    public int CommunityId { get; set; }
}
