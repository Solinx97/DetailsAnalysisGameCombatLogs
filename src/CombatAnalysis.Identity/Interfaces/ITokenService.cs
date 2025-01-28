namespace CombatAnalysis.Identity.Interfaces;

public interface ITokenService
{
    Task RemoveExpiredTokensAsync();
}
