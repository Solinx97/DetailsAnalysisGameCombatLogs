namespace CombatAnalysis.CommunicationDAL.Entities.Post;

public class UserPostLike
{
    public int Id { get; set; }

    public int UserPostId { get; set; }

    public string AppUserId { get; set; }
}
