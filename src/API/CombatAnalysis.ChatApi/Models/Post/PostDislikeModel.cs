namespace CombatAnalysis.ChatApi.Models.Post;

public class PostDislikeModel
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string CustomerId { get; set; }
}
