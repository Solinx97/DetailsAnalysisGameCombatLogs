using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Helpers;

internal class CombatParserHelper : ICombatParserHelper
{
    public string[] SplitCombatData(string combatData)
    {
        var log = combatData.Split("  ");
        var parse = log[1].Split(',');

        var data = new List<string>
        {
            log[0],
        };

        data.AddRange(parse);

        CheckComplexText(data);

        return [.. data];
    }

    public CombatUnit ParseUnits(ConcurrentDictionary<string, CombatUnit> units, string gameId, string name, string unitHash,  string? creatorGameId = null)
    {
        var type = CombatUnitType.EnemyCreature;
        if (gameId.Contains(CombatLogKeyWords.Creature) && creatorGameId != null && creatorGameId.Contains(CombatLogKeyWords.Player))
        {
            type = CombatUnitType.PlayerCreature;
        }
        else if (gameId.Contains(CombatLogKeyWords.Vehicle))
        {
            type = CombatUnitType.Vehicle;
        }
        else if (gameId.Contains(CombatLogKeyWords.Player))
        {
            type = CombatUnitType.Player;
        }
        else if (gameId.Contains(CombatLogKeyWords.Pet))
        {
            type = CombatUnitType.Pet;
        }

        var unit = new CombatUnit
        {
            GameId = gameId,
            Name = name.Trim('"'),
            CreatorGameId = creatorGameId,
            UnitHash = unitHash,
            Type = (int)type,
        };

        units.TryAdd(gameId, unit);

        return unit;
    }

    private static void CheckComplexText(List<string> content)
    {
        var craft = string.Empty;
        var startIndex = -1;
        var finishIndex = -1;
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i].StartsWith('\"') && !content[i].EndsWith('\"'))
            {
                craft += content[i];
                startIndex = i;
            }
            else if (!string.IsNullOrEmpty(craft) && !content[i].EndsWith('\"'))
            {
                craft += content[i];
            }
            else if (!string.IsNullOrEmpty(craft) && content[i].EndsWith('\"'))
            {
                craft += content[i];
                finishIndex = i;
                break;
            }
        }

        if (startIndex >= 0 && startIndex + 1 < content.Count && finishIndex >= 0)
        {
            content[startIndex] = craft;
            content.RemoveRange(startIndex + 1, finishIndex - startIndex);
        }
    }
}
