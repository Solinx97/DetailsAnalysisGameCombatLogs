using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Entities.WoWMoPClassic;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;
using CombatAnalysis.WoW_5_5_4.CombatParser.Details;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Services;

internal class CombatParserService(IFileManager fileManager, ILogger<CombatParserService> logger, IHttpClientHelper httpHelper) : WoW.CombatParser.Services.CombatParserService(fileManager, logger, httpHelper), Interfaces.ICombatParserService
{
    protected override async Task GetCombatInformationAsync(string[] builtCombat, Dictionary<string, List<string>> petsId)
    {
        var combat = CreateCombat(builtCombat, petsId);
        if (combat == null)
        {
            return;
        }

        var duration = combat.FinishDate - combat.StartDate;
        if (duration < CombatLogKeyWords.MinCombatDuration)
        {
            return;
        }

        var combatDetails = new CombatDetails(_logger, combat.PetsId);

        var players = await GetCombatPlayers(combat, combatDetails);
        combat.CombatPlayers = [.. players];

        combat.Units = [.. combatDetails.Units.Values];
        combat.UnitCasts = [.. combatDetails.UnitCasts.Values.SelectMany(x => x)];
        combat.UnitPositions = [.. combatDetails.UnitPositions.Values.SelectMany(x => x)];

        CalculatingCommonCombatDetails(combat);

        AddNewCombat(combat);
    }

    protected override IPlayerStats GetStats(string[] combatInfo)
    {
        var stats = new WoWMoPClassicPlayerStats
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

    protected override List<CombatPlayerPreAura> GetPreAuras(string preAurasInformation)
    {
        var allPreAuras = preAurasInformation.Split(',');
        var preAuras = new List<CombatPlayerPreAura>();
        for (var i = 0; i + 2 < allPreAuras.Length; i += 3)
        {
            var preAura = new CombatPlayerPreAura
            {
                CreatorGameId = allPreAuras[i],
                GameId = int.Parse(allPreAuras[i + 1]),
                Status = int.Parse(allPreAuras[i + 2]),
            };
            preAuras.Add(preAura);
        }

        return preAuras;
    }
}
