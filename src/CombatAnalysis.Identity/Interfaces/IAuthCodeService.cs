namespace CombatAnalysis.Identity.Interfaces;

public interface IAuthCodeService
{
    void RemoveExpiredCodes();
}
