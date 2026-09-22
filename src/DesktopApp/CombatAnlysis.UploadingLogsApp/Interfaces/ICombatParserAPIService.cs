using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Interfaces;

public interface ICombatParserAPIService
{
    Task<int> SaveCombatLogAsync(List<CreateCombatModel> combats, LogType logType, CancellationToken cancellationToken);

    Task SaveAsync(List<CreateCombatModel> combats, int combatLogId, Action<string, string, string> combatUploaded, Func<CancellationToken> requestCancelationToken);

    Task GetBossAsync(List<CreateCombatModel> combats, bool useDefault, CancellationToken cancellationToken);
}
