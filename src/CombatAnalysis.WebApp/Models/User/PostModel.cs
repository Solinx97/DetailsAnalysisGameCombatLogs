namespace CombatAnalysis.WebApp.Models.User;

public class PostModel
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public string OwnerId { get; set; }
}
