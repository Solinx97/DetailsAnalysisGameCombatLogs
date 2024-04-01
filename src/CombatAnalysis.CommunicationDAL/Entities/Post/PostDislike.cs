namespace CombatAnalysis.CommunicationDAL.Entities.Post;

public class PostDislike
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string CustomerId { get; set; }
}
