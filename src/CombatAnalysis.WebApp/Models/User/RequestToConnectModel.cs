namespace CombatAnalysis.WebApp.Models.User;

public class RequestToConnectModel
{
    public int Id { get; set; }

    public string ToAppUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; }
}
