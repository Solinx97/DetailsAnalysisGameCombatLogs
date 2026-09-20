using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces.Data;
using CombatAnalysis.UploadingLogsApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace CombatAnalysis.UploadingLogsApp.Services.Data;

internal class CombatService : ICombatService
{
    public List<CreateCombatModel> Combats { private get; set; } = [];

    public LogType LogType { get; set; }

    public List<CreateCombatModel> GetCombats(bool isOnlySupported)
    {
        List<CreateCombatModel> combats = [.. Combats.Select(x =>
        {
            if (x.Boss.Id > 0)
            {
                x.IsSupported = true;
            }

            return x;
        }).Where(x => isOnlySupported ? x.IsSupported : x.IsSupported || !x.IsSupported)];

        return combats;
    }

    public void Clear()
    {
        Combats.Clear();
    }
}
