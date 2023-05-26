namespace CombatAnalysis.DAL.Entities.User;

public class PostDislike
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string OwnerId { get; set; }
}
