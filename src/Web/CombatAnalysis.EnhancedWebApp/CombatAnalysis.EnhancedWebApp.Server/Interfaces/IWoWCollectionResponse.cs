using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces;

public interface IWoWCollectionResponse
{
    WoWGameDataEntityModel[] Items { get; }
}
