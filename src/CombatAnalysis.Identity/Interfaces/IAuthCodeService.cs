namespace CombatAnalysis.Identity.Interfaces;

public interface IAuthCodeService
{
    Task RemoveExpiredCodesAsync();
}
