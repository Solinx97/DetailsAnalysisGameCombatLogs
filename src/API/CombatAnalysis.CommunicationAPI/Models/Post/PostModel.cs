namespace CombatAnalysis.CommunicationAPI.Models.Post;

public class PostModel
{
    public int Id { get; set; }

    public string Owner { get; set; }

    public string Content { get; set; }

    public int PostType { get; set; }

    public string Tags { get; set; }

    public DateTimeOffset When { get; set; }

    public int LikeCount { get; set; }

    public int DislikeCount { get; set; }

    public int CommentCount { get; set; }

    public string CustomerId { get; set; }
}
