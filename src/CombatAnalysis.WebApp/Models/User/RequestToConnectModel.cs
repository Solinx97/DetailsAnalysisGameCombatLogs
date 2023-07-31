namespace CombatAnalysis.WebApp.Models.User;

public class RequestToConnectModel
{
    public int Id { get; set; }

    public string ToUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public string OwnerId { get; set; }
}
