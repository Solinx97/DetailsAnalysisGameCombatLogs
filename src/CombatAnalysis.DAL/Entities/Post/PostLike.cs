namespace CombatAnalysis.DAL.Entities.Post;

public class PostLike
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string CustomerId { get; set; }
}
