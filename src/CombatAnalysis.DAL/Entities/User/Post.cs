namespace CombatAnalysis.DAL.Entities.User;

public class Post
{
    public int Id { get; set; }

    public string Description { get; set; }

    public DateTimeOffset When { get; set; }

    public string OwnerId { get; set; }
}
