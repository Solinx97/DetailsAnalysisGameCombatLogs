namespace CombatAnalysis.WebApp.Models.User;

public class PostLikeModel
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string OwnerId { get; set; }
}
