namespace CombatAnalysis.CommunicationAPI.Models.Post;

public class UserPostCommentModel
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int UserPostId { get; set; }

    public string AppUserId { get; set; }
}
