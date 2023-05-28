namespace CombatAnalysis.DAL.Entities.User;

public class RequestToConnect
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string ToUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public int Result { get; set; }

    public string OwnerId { get; set; }
}
