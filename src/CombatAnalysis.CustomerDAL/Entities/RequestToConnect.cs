namespace CombatAnalysis.CustomerDAL.Entities;

public class RequestToConnect
{
    public int Id { get; set; }

    public string ToAppUserId { get; set; }

    public DateTimeOffset When { get; set; }

    public string AppUserId { get; set; }
}
