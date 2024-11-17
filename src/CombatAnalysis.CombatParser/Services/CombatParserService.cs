using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Patterns;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace CombatAnalysis.CombatParser.Services;

public class CombatParserService
{
    private readonly IList<Interfaces.IObserver<Combat>> _combatObservers;
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
        _combatObservers = new List<Interfaces.IObserver<Combat>>();
        _zones = new List<PlaceInformation>();
    }

    public List<Combat> Combats { get; }

    public Dictionary<string, List<string>> PetsId { get; private set; }

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
        PetsId?.Clear();
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

            PetsId = petsId;

            GetCombatPlayersData(combat, petsId);

            BaseCombatDetails combatDetailsDeaths = new CombatDetailsDeaths(_logger, combat.Players, combat);
            combat.DeathNumber = combatDetailsDeaths.GetData(string.Empty, combat.Data);

            CalculatingCommonCombatDetails(combat);

            AddNewCombat(combat);
        }
        catch (IndexOutOfRangeException ex)
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

    private void GetCombatPlayersData(Combat combat, Dictionary<string, List<string>> petsId)
    {
        var players = GetCombatPlayers(combat.Data, combat, petsId);
        combat.Players = players;
    }

    private static void CalculatingCommonCombatDetails(Combat combat)
    {
        var players = combat.Players;

        combat.DamageDone = players.Sum(player => player.DamageDone);
        combat.HealDone = players.Sum(player => player.HealDone);
        combat.DamageTaken = players.Sum(player => player.DamageTaken);
        combat.EnergyRecovery = players.Sum(player => player.EnergyRecovery);
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

    private List<CombatPlayer> GetCombatPlayers(List<string> combatInformation, Combat combat, Dictionary<string, List<string>> petsId)
    {
        var playersIdAndStats = new ConcurrentDictionary<string, string>();
        var combatInfoByKeyWord = combatInformation
            .Where(info => info.Contains(CombatLogKeyWords.CombatantInfo))
            .ToList();

        foreach (var info in combatInfoByKeyWord)
        {
            var combatantInfo = info.Split("  ")[1];
            var playerCombatantInfoArray = combatantInfo.Split(',');

            playersIdAndStats.TryAdd(playerCombatantInfoArray[1], combatantInfo);
        }

        var players = new ConcurrentBag<CombatPlayer>();

        Parallel.ForEach(playersIdAndStats, item =>
        {
            var playerCombatantInfoArray = item.Value.Split(new char[2] { '[', ']' });

            var username = GetCombatPlayerUsernameById(combatInformation, item.Key);
            var averageItemLevel = GetAverageItemLevel(playerCombatantInfoArray[1]);
            int usedBuffs = GetUsedBuffs(playerCombatantInfoArray[3]);

            var combatDetailsDamageDone = new CombatDetailsDamageDone(_logger)
            {
                PetsId = petsId
            };

            var combatDetailsHealDone = new CombatDetailsHealDone(_logger);
            var combatDetailsDamageTaken = new CombatDetailsDamageTaken(_logger);
            var combatDetailsResourceRecovery = new CombatDetailsResourceRecovery(_logger);

            var combatPlayerData = new CombatPlayer
            {
                Username = username,
                PlayerId = item.Key,
                AverageItemLevel = double.Round(averageItemLevel, 2),
                EnergyRecovery = combatDetailsResourceRecovery.GetData(item.Key, combat.Data),
                DamageDone = combatDetailsDamageDone.GetData(item.Key, combat.Data),
                HealDone = combatDetailsHealDone.GetData(item.Key, combat.Data),
                DamageTaken = combatDetailsDamageTaken.GetData(item.Key, combat.Data),
                UsedBuffs = usedBuffs,
            };

            players.Add(combatPlayerData);
        });

        return players.ToList();
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

    private static int GetUsedBuffs(string buffsInformation)
    {
        var splitInformations = buffsInformation.Split(",");
        var countOfBuffs = splitInformations.Length / 2;

        return countOfBuffs;
    }

    private static double GetAverageItemLevel(string equipmentsInformation)
    {
        var splitEquipementsInformation = equipmentsInformation.Split("))");
        splitEquipementsInformation = splitEquipementsInformation.Select(x => x.TrimStart(',')).ToArray();
        splitEquipementsInformation = splitEquipementsInformation.Select(x => x.TrimStart('(')).ToArray();
        splitEquipementsInformation = splitEquipementsInformation.Select(x => x.Insert(x.Length, ")")).ToArray();

        var ilvl = new List<int>();
        for (var i = 0; i < splitEquipementsInformation.Length - 1; i++)
        {
            var equipmentIlvlInformation = splitEquipementsInformation[i].Split(',')[1];
            if (int.TryParse(equipmentIlvlInformation, out var equipmentIlvl))
            {
                ilvl.Add(equipmentIlvl);
            }
        }

        var averageILvl = ilvl.Average();
        return averageILvl;
    }
}
