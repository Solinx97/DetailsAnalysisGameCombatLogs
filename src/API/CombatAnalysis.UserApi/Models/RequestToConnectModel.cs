namespace CombatAnalysis.UserApi.Models;

public class RequestToConnectModel
{
    public int Id { get; set; }

    public string ToUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public string CustomerId { get; set; }
}
