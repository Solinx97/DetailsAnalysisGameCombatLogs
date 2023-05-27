namespace CombatAnalysis.UserApi.Models;

public class PostModel
{
    public int Id { get; set; }

    public string Description { get; set; }

    public DateTimeOffset When { get; set; }

    public string OwnerId { get; set; }
}
