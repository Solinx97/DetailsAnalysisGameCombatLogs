using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IMountService
{
    Task<MountModel[]> GetUserMountsAsync(string regionName, CancellationToken cancellationToken);
}
