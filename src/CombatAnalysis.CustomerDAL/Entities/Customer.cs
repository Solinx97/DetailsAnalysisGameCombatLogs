namespace CombatAnalysis.CustomerDAL.Entities;

public class Customer
{
    public string Id { get; set; }

    public string Country { get; set; }

    public string City { get; set; }

    public int PostalCode { get; set; }

    public string AppUserId { get; set; }
}
