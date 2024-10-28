namespace CombatAnalysis.IdentityDAL.Entities;

public class ResetToken
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Token { get; set; }

    public DateTime ExpirationTime { get; set; }

    public bool IsUsed { get; set; }
}
