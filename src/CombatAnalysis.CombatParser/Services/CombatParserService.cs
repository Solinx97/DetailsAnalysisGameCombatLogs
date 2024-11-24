using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace CombatAnalysis.CombatParser.Services;

public class CombatParserService
{
    private readonly IList<PlaceInformation> _zones;
    private readonly IFileManager _fileManager;
    private readonly ILogger _logger;

    private readonly TimeSpan _minCombatDuration = TimeSpan.Parse("00:00:20");

    private int _combatNumber = 0;

    public CombatParserService(IFileManager fileManager, ILogger logger)
    {
        _fileManager = fileManager;
        _logger = logger;

        Combats = new List<Combat>();
        _zones = new List<PlaceInformation>();
    }

    public List<Combat> Combats { get; }

    public async Task<bool> FileCheckAsync(string combatLog)
    {
        var fileIsCorrect = true;

        using var reader = _fileManager.StreamReader(combatLog);
        var line = await reader.ReadLineAsync();
        if (!line.Contains(CombatLogKeyWords.CombatLogVersion))
        {
            fileIsCorrect = false;
        }

        return fileIsCorrect;
    }

    public async Task ParseAsync(string combatLogPath)
    {
        var newCombatFromLogs = new StringBuilder();
        var petsId = new Dictionary<string, List<string>>();
        var bossCombatStarted = false;

        Clear();

        try
        {
            var lines = await File.ReadAllLinesAsync(combatLogPath);
            ProcessCombatLogLines(lines, petsId, ref bossCombatStarted, newCombatFromLogs);
        }
        catch (Exception ex)
        {
            var message = $"Error reading file: {ex.Message}";
            _logger.LogError(message);
        }
    }

    public void Clear()
    {
        Combats.Clear();
        _zones.Clear();
        _combatNumber = 0;
    }

    private void ProcessCombatLogLines(string[] lines, Dictionary<string, List<string>> petsId, ref bool bossCombatStarted, StringBuilder newCombatFromLogs)
    {
        foreach (var line in lines)
        {
            ProcessLine(line, ref bossCombatStarted, newCombatFromLogs, petsId);
        }
    }

    private void ProcessLine(string line, ref bool bossCombatStarted, StringBuilder newCombatFromLogs, Dictionary<string, List<string>> petsId)
    {
        if (line.Contains(CombatLogKeyWords.SpellSummon))
        {
            ParseGetPets(line, petsId);
        }

        if (line.Contains(CombatLogKeyWords.ZoneChange))
        {
            GetZoneName(line);
        }

        if (line.Contains(CombatLogKeyWords.EncounterStart))
        {
            bossCombatStarted = true;
            newCombatFromLogs.AppendLine(line);
        }

        if (!bossCombatStarted)
        {
            return;
        }

        if (line.Contains(CombatLogKeyWords.EncounterEnd))
        {
            bossCombatStarted = false;

            newCombatFromLogs.AppendLine(line);

            var newCombatFromLogsString = newCombatFromLogs.ToString();
            var combatInformationList = newCombatFromLogsString.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

            GetCombatInformation(combatInformationList, petsId);

            newCombatFromLogs.Clear();
        }
        else
        {
            newCombatFromLogs.AppendLine(line);
        }
    }

    private static void ParseGetPets(string data, Dictionary<string, List<string>> petsId)
    {
        var combatLogParts = data.Split("  ")[1].Split(',');
        var playerId = combatLogParts[1].Contains(CombatLogKeyWords.Player) ? combatLogParts[1] : string.Empty;

        if (string.IsNullOrEmpty(playerId))
        {
            return;
        }

        var petId = combatLogParts[1].Contains(CombatLogKeyWords.Player) ? combatLogParts[5] : string.Empty;
        if (!petsId.TryGetValue(playerId, out var petList))
        {
            petList = new List<string>();
            petsId[playerId] = petList;
        }

        petList.Add(petId);
    }

    private void GetCombatInformation(List<string> builtCombat, Dictionary<string, List<string>> petsId)
    {
        try
        {
            if (!builtCombat[^1].Contains(CombatLogKeyWords.EncounterEnd))
            {
                return;
            }

            var combat = new Combat
            {
                Name = GetCombatName(builtCombat[0]),
                Difficulty = GetDifficulty(builtCombat[0]),
                Data = builtCombat,
                IsWin = GetCombatResult(builtCombat[^1]),
                StartDate = GetTime(builtCombat[0]),
                FinishDate = GetTime(builtCombat[^1]),
                PetsId = petsId,
            };

            var duration = combat.FinishDate - combat.StartDate;
            if (duration < _minCombatDuration)
            {
                return;
            }

            var players = GetCombatPlayers(combat, petsId);
            combat.Players = players;

            var combatDetailsDeaths = new CombatDetailsDeaths(_logger, combat.Players, combat);
            combat.DeathNumber = combatDetailsDeaths.GetDeathNumber(combat.Data);

            CalculatingCommonCombatDetails(combat);

            AddNewCombat(combat);
        }
        catch (IndexOutOfRangeException ex)
        {
            var message = $"Error parsing data from file: {ex.Message}";
            _logger.LogError(message);
        }
        catch (Exception ex)
        {
            var message = $"Error parsing data from file: {ex.Message}";
            _logger.LogError(message);
        }
    }

    private static string GetCombatName(string combatStart)
    {
        var data = combatStart.Split("  ")[1];
        var name = data.Split(',')[2];
        var clearName = name.Trim('"');

        return clearName;
    }

    private static int GetDifficulty(string combatStart)
    {
        var data = combatStart.Split("  ")[1];
        var difficulty = data.Split(',')[3];

        if (int.TryParse(difficulty, out var diff))
        {
            return diff;
        }

        return 0;
    }

    private static bool GetCombatResult(string combatFinish)
    {
        var data = combatFinish.Split("  ");
        var split = data[1].Split(',');
        var combatResult = int.Parse(split[split.Length - 1]);
        var isWin = combatResult == 1;

        return isWin;
    }

    private static DateTimeOffset GetTime(string combatStart)
    {
        var parse = combatStart.Split("  ")[0];
        var combatDate = parse.Split(' ');
        var dateWithoutTime = combatDate[0].Split('/');
        var time = combatDate[1].Split('.')[0];
        var clearDate = $"{dateWithoutTime[0]}/{dateWithoutTime[1]}/{DateTimeOffset.UtcNow.Year} {time}";

        DateTimeOffset.TryParse(clearDate, CultureInfo.GetCultureInfo("en-EN"), DateTimeStyles.AssumeUniversal, out var date);

        return date.UtcDateTime;
    }

    private static void CalculatingCommonCombatDetails(Combat combat)
    {
        var players = combat.Players;

        combat.DamageDone = players.Sum(player => player.DamageDone);
        combat.HealDone = players.Sum(player => player.HealDone);
        combat.DamageTaken = players.Sum(player => player.DamageTaken);
        combat.EnergyRecovery = players.Sum(player => player.ResourcesRecovery);
    }

    private void AddNewCombat(Combat combat)
    {
        foreach (var item in _zones)
        {
            if (item.EntryDate < combat.StartDate)
            {
                combat.DungeonName = item.Name;
            }
        }

        combat.LocallyNumber = _combatNumber;
        Combats.Add(combat);
    }

    private List<CombatPlayer> GetCombatPlayers(Combat combat, Dictionary<string, List<string>> petsId)
    {
        var combatInformations = combat.Data
            .Where(info => info.Contains(CombatLogKeyWords.CombatantInfo))
            .ToList();

        var players = new List<CombatPlayer>();

        foreach (var item in combatInformations)
        {
            var combatInfoToArray = item.Split(new char[2] { ' ', ',' });
            var combatEquipmentsInfoToArray = item.Split(new char[2] { '[', ']' });

            var username = GetCombatPlayerUsernameById(combat.Data, combatInfoToArray[4]);
            var averageItemLevel = GetAverageItemLevel(combatEquipmentsInfoToArray[1]);
            //int usedBuffs = GetUsedBuffs(combatInfoToArray[3]);

            var combatPlayerData = new CombatPlayer
            {
                Username = username,
                PlayerId = combatInfoToArray[4],
                AverageItemLevel = double.Round(averageItemLevel, 2),
                UsedBuffs = 0,
            };

            players.Add(combatPlayerData);
        }

        var playersId = players.Select(x => x.PlayerId).ToList();

        var combatDetails = new CombatDetails(_logger, petsId);
        combatDetails.Calculate(playersId, combat.Data);

        foreach (var item in players)
        {
            item.DamageDone = combatDetails.DamageDone[item.PlayerId].Sum(x => x.Value);
            item.HealDone = combatDetails.HealDone[item.PlayerId].Sum(x => x.Value);
            item.DamageTaken = combatDetails.DamageTaken[item.PlayerId].Sum(x => x.Value);
            item.ResourcesRecovery = combatDetails.ResourcesRecovery[item.PlayerId].Sum(x => x.Value);
        }

        return players;
    }

    private void GetZoneName(string combatLog)
    {
        var parse = combatLog.Split("  ")[1];
        var name = parse.Split(',')[2];
        var clearName = name.Trim('"');

        var date = GetTime(combatLog);

        var zone = new PlaceInformation
        {
            Name = clearName,
            EntryDate = date
        };

        _zones.Add(zone);
    }

    private static string GetCombatPlayerUsernameById(List<string> combatInformation, string playerId)
    {
        var parsedUsername = string.Empty;
        for (var i = 1; i < combatInformation.Count; i++)
        {
            var data = combatInformation[i].Split(',');
            if (!combatInformation[i].Contains(CombatLogKeyWords.CombatantInfo)
                && playerId == data[1])
            {
                var username = data[2];
                parsedUsername = username.Trim('"');
                break;
            }
        }

        return parsedUsername;
    }

    //private static int GetUsedBuffs(string buffsInformation)
    //{
    //    var splitInformations = buffsInformation.Split(",");
    //    var countOfBuffs = splitInformations.Length / 2;

    //    return countOfBuffs;
    //}

    private static double GetAverageItemLevel(string equipmentsInformation)
    {
        var splitEquipementsInformation = equipmentsInformation.Split("))");

        var ilvl = new List<int>();
        for (var i = 0; i < splitEquipementsInformation.Length - 1; i++)
        {
            var equipmentIlvlInformation = splitEquipementsInformation[i].Split(',')[1];
            if (int.TryParse(equipmentIlvlInformation, out var equipmentIlvl) && equipmentIlvl > 0)
            {
                ilvl.Add(equipmentIlvl);
            }
        }

        var averageILvl = ilvl.Average();
        return averageILvl;
    }
}
