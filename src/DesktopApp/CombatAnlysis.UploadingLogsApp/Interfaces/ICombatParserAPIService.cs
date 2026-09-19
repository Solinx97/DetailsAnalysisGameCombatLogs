using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface ICombatParserAPIService
{
    Task SaveAsync(List<CombatModel> combats, int combatLogId, Action<string, string, string> combatUploaded, Func<CancellationToken> requestCancelationToken);

    Task<int> SaveCombatLogAsync(List<CombatModel> combats, LogType logType, CancellationToken cancellationToken);

    Task GetBossAsync(List<CombatModel> combats, bool useDefault, CancellationToken cancellationToken);
}
