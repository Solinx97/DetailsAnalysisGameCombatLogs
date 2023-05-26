namespace CombatAnalysis.DAL.Entities.User;

public class PostLike
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string OwnerId { get; set; }
}
