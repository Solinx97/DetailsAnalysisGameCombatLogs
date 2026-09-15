using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;
using CombatAnalysis.WoW_12_1_0.CombatParser.Details;
using CombatAnalysis.WoW_12_1_0.CombatParser.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Services;

internal class CombatParserService(ICombatParserHelper combatParserHelper, IFileManager fileManager, 
    ILogger<CombatParserService> logger, IHttpClientHelper httpHelper) 
    : WoW.CombatParser.Services.CombatParserService(combatParserHelper, fileManager, logger, httpHelper), Interfaces.ICombatParserService
{
    protected override CombatDetails GetCombatDetails(ConcurrentDictionary<string, Unit> units)
    {
        return new CombatDetails(_combatParserHelper, _logger, units);
    }

    protected override async Task<CombatPlayer> GetCombatPlayerDataAsync(string combatInformation, string[] combatData, ConcurrentDictionary<string, Unit> units)
    {
        var combatInformationParams = combatInformation.Split(',');
        var combatPlayerParams = combatInformation.Split(['[', ']']);
        var equipments = combatPlayerParams[3];
        var preAuras = combatPlayerParams[5];

        var statsInformation = combatInformationParams.Skip(3).Take(23).ToArray();

        var combatPlayer = await CreateCombatPlayerAsync(statsInformation, combatData, combatInformationParams, preAuras, equipments, units);

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
            Dodge = int.Parse(combatInfo[4]),
            Parry = int.Parse(combatInfo[5]),
            Block = int.Parse(combatInfo[6]),
            Crit = int.Parse(combatInfo[8]),
            Movement = int.Parse(combatInfo[11]),
            Lifesteal = int.Parse(combatInfo[12]),
            Haste = int.Parse(combatInfo[13]),
            Avoidance = int.Parse(combatInfo[16]),
            Mastery = int.Parse(combatInfo[17]),
            Versality = int.Parse(combatInfo[18]),
            Armor = int.Parse(combatInfo[21]),
        };

        //var segment = new ArraySegment<string>(combatInfo, 23, 6);
        //var talents = string.Join(',', segment);
        //stats.Talents = talents;

        return stats;
    }
}
