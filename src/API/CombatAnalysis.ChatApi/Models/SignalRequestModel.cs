namespace CombatAnalysis.ChatApi.Models;

public class SignalRequestModel
{
    public int Room { get; set; }

    public string ConnectionId { get; set; }

    public string Data { get; set; }
}
