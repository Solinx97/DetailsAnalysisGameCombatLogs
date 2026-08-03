using CombatAnalysis.Core.Models.GameLogs;

namespace CombatAnalysis.Core.Interfaces;

public interface ICombatParserAPIService
{
    Task DeleteCombatLogByUserAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync(int logType, string? appUserId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId, CancellationToken cancellationToke);

    Task<int> LoadCountAsync(string address, CancellationToken cancellationToken);
}
