namespace CombatAnalysis.UserApi.Models;

public class PostDislikeModel
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public string OwnerId { get; set; }
}
