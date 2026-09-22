using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

namespace CombatAnalysis.Core.Interfaces;

public interface ICombatParserAPIService
{
    Task DeleteCombatLogByUserAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync(int logType, int gameVersion, string? appUserId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId, CancellationToken cancellationToke);

    Task<int> LoadCountAsync(string address, CancellationToken cancellationToken);

    Task<CombatPlayerStatsModel?> LoadPlayerStatsAsync(string address, CancellationToken cancellationToken);

    Task<List<string>> LoadFilterItemsAsync(string address, CancellationToken cancellationToken);
}
