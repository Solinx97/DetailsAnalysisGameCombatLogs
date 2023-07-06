namespace CombatAnalysis.CustomerBL.DTO;

public class AppUserDto
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTimeOffset Birthday { get; set; }

    public string Password { get; set; }
}
