namespace CombatAnalysis.CustomerDAL.Entities;

public class BannedUser
{
    public int Id { get; set; }

    public string BannedCustomerId { get; set; }

    public string CustomerId { get; set; }
}
