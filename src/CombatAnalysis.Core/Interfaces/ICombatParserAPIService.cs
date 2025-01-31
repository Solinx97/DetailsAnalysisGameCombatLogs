using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Models;

namespace CombatAnalysis.Core.Interfaces;

public interface ICombatParserAPIService
{
    void SetUpPort();

    Task<bool> SaveAsync(List<CombatModel> combats, CombatLogModel combatLog, Action<int, string, string> combatUploaded, CancellationToken cancellationToken);

    Task DeleteCombatLogByUserAsync(int id);

    Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync();

    Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync(List<int> combatLogsId);

    Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId);

    Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId);

    Task<int> LoadCountAsync(string address);

    Task<CombatLogModel> SaveCombatLogAsync(List<CombatModel> combats, LogType logType, CancellationToken cancellationToken);
}
