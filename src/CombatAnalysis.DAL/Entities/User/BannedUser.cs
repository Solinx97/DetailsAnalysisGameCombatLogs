namespace CombatAnalysis.DAL.Entities.User;

public class BannedUser
{
    public int Id { get; set; }

    public string BannedCustomerId { get; set; }

    public string CustomerId { get; set; }
}
