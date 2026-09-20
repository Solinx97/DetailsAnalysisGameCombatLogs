using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Models;
using System.Collections.Generic;

namespace CombatAnalysis.UploadingLogsApp.Interfaces.Data;

public interface ICombatService
{
    List<CreateCombatModel> Combats { set; }

    LogType LogType { get; set; }

    List<CreateCombatModel> GetCombats(bool isOnlySupported);

    void Clear();
}
