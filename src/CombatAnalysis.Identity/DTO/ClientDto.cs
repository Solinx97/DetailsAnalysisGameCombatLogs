namespace CombatAnalysis.Identity.DTO;

public class ClientDto
{
    public string Id { get; set; }

    public string RedirectUrl { get; set; }

    public string Scope { get; set; }

    public string ClientName { get; set; }

    public string ClientType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
