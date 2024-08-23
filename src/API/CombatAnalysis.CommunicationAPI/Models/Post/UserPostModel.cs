namespace CombatAnalysis.CommunicationAPI.Models.Post;

public class UserPostModel
{
    public int Id { get; set; }

    public string Owner { get; set; }

    public string Content { get; set; }

    public int PublicType { get; set; }

    public string Tags { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public int CommentCount { get; set; }

    public string AppUserId { get; set; }
}
