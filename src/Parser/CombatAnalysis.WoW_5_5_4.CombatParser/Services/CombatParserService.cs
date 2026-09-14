using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;
using CombatAnalysis.WoW_5_5_4.CombatParser.Details;
using CombatAnalysis.WoW_5_5_4.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Services;

internal class CombatParserService(ICombatParserHelper combatParserHelper, IFileManager fileManager,
    ILogger<CombatParserService> logger, IHttpClientHelper httpHelper) 
    : WoW.CombatParser.Services.CombatParserService(combatParserHelper, fileManager, logger, httpHelper), Interfaces.ICombatParserService
{
    protected override async Task GetCombatInformationAsync(string[] builtCombat, ConcurrentDictionary<string, Unit> units)
    {
        var combat = CreateCombat(builtCombat);
        if (combat == null)
        {
            return;
        }

        var duration = combat.FinishDate - combat.StartDate;
        if (duration < CombatLogKeyWords.MinCombatDuration)
        {
            return;
        }

        var combatDetails = new CombatDetails(_combatParserHelper, _logger, units);

        var players = await GetCombatPlayers(builtCombat, combat.Duration, combat.StartDate, combat.FinishDate, combatDetails);
        combat.CombatPlayers = [.. players];

        combat.Units = [.. combatDetails.Units.Values.Where(x => x.UnitHealthes.Count > 0 && x.UnitPositions.Count > 0)];

        CalculatingCommonCombatDetails(combat);

        AddNewCombat(combat);
    }

    protected override async Task<CombatPlayer> CreateCombatPlayerAsync(string combatInformation, string[] combatData)
    {
        var combatInformationParams = combatInformation.Split(',');
        var combatPlayerParams = combatInformation.Split(['[', ']']);
        var equipments = combatPlayerParams[1];
        var preAuras = combatPlayerParams[3];

        var statsInformation = combatInformationParams.Skip(3).Take(30).ToArray();

        var combatPlayer = await CreateCombatPlayerAsync(statsInformation, combatData, combatInformationParams, preAuras, equipments);

        return combatPlayer;
    }

    protected override IPlayerStats GetStats(string[] combatInfo)
    {
        var stats = new PlayerStats
        {
            Strength = int.Parse(combatInfo[0]),
            Agility = int.Parse(combatInfo[1]),
            Stamina = int.Parse(combatInfo[2]),
            Intelligence = int.Parse(combatInfo[3]),
            Spirit = int.Parse(combatInfo[4]),
            Dodge = int.Parse(combatInfo[5]),
            Parry = int.Parse(combatInfo[6]),
            Block = int.Parse(combatInfo[7]),
            Crit = int.Parse(combatInfo[8]),
            Haste = int.Parse(combatInfo[11]),
            Hit = int.Parse(combatInfo[14]),
            Expertise = int.Parse(combatInfo[15]),
            Armor = int.Parse(combatInfo[16]),
        };

        var segment = new ArraySegment<string>(combatInfo, 23, 6);
        var talents = string.Join(',', segment);
        stats.Talents = talents;

        return stats;
    }
}
