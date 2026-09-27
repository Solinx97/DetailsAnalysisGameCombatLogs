using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IMountService
{
    Task<MountModel[]> GetAccountMountsAsync(string regionName, CancellationToken cancellationToken);
}
