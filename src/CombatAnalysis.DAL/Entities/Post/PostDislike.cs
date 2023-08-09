namespace CombatAnalysis.DAL.Entities.Post;

public class PostDislike
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string OwnerId { get; set; }
}
