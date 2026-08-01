using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface ICombatParserAPIService
{
    Task SaveAsync(List<CombatModel> combats, CombatLogModel combatLog, Action<string, string> combatUploaded, Func<CancellationToken> requestCancelationToken);

    Task DeleteCombatLogByUserAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync(CancellationToken cancellationToken);

    Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId, CancellationToken cancellationToken);

    Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId, CancellationToken cancellationToke);

    Task<int> LoadCountAsync(string address, CancellationToken cancellationToken);

    Task<CombatLogModel> SaveCombatLogAsync(List<CombatModel> combats, LogType logType, CancellationToken cancellationToken);

    Task GetBossAsync(List<CombatModel> combats, CancellationToken cancellationToken);
}
