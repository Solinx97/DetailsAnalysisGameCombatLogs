namespace CombatAnalysisIdentity.Models;

public class CustomerModel
{
    public string Id { get; set; } = string.Empty;

    public string Country { get; set; }

    public string City { get; set; }

    public int PostalCode { get; set; }

    public string AppUserId { get; set; }
}
