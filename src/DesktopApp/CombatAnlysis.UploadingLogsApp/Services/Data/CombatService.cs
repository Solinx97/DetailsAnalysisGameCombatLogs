using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Interfaces.Data;
using CombatAnalysis.UploadingLogsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

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
        foreach (var combat in Combats)
        {
            ClearCombat(combat);
        }

        Combats.Clear();

        // Call GC to collect and release LOH right now
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);
    }

    private static void ClearCombat(CreateCombatModel combat)
    {
        foreach (var unit in combat.Units)
        {
            unit.UnitCasts.Clear();
            unit.UnitPositions.Clear();
            unit.UnitHealthes.Clear();
            unit.Auras.Clear();
            unit.PreAuras.Clear();
            unit.DamageDones.Clear();
            unit.DamageTakens.Clear();
            unit.HealDones.Clear();
            unit.ResourceRecoveries.Clear();
        }

        combat.CombatPlayers.Clear();
        combat.Units.Clear();
    }
}
