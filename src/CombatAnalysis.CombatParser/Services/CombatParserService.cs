using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Patterns;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace CombatAnalysis.CombatParser.Services;

public class CombatParserService : IParser
{
    private readonly IList<IObserver> _observers;
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
        _observers = new List<IObserver>();
        _zones = new List<PlaceInformation>();
    }

    public List<Combat> Combats { get; }

    public async Task<bool> FileCheck(string combatLog)
    {
        using var reader = _fileManager.StreamReader(combatLog);
        var fileIsCorrect = true;
        var line = await reader.ReadLineAsync();
        if (!line.Contains(CombatLogKeyWords.CombatLogVersion))
        {
            fileIsCorrect = false;
        }

        return fileIsCorrect;
    }

    public async Task Parse(string combatLog)
    {
        Combats.Clear();

        using var reader = _fileManager.StreamReader(combatLog);
        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line.Contains(CombatLogKeyWords.EncounterStart))
            {
                await GetCombatInformation(line, reader);
            }
            else if (line.Contains(CombatLogKeyWords.ZoneChange))
            {
                GetDungeonName(line);
            }
        }
    }

    private async Task GetCombatInformation(string line, StreamReader reader)
    {
        try
        {
            var combatData = await GetCombatData(reader);
            combatData.Insert(0, line);

            var combat = new Combat
            {
                Name = GetCombatName(combatData[0]),
                Difficulty = GetDifficulty(combatData[0]),
                DungeonSize = GetDungeonSize(combatData[0]),
                Data = combatData,
                IsWin = GetCombatResult(combatData[^1]),
                StartDate = GetTime(combatData[0]),
                FinishDate = GetTime(combatData[^1]),
            };

            var duration = combat.FinishDate - combat.StartDate;
            if (duration < _minCombatDuration)
            {
                return;
            }

            GetCombatPlayersData(combat);

            CombatDetailsTemplate combatDetailsDeaths = new CombatDetailsDeaths(_logger, combat.Players);
            combat.DeathNumber = combatDetailsDeaths.GetData(string.Empty, combat.Data);

            CalculatingCommonCombatDetails(combat);

            AddNewCombat(combat);
        }
        catch (IndexOutOfRangeException)
        {
            var combat = new Combat();
            AddNewCombat(combat);
        }
    }

    private async Task<List<string>> GetCombatData(StreamReader reader)
    {
        string line;
        var combatElements = new List<string>();
        while ((line = await reader.ReadLineAsync()) != null)
        {
            combatElements.Add(line);
            if (line.Contains(CombatLogKeyWords.EncounterEnd))
            {
                break;
            }
        }

        return combatElements;
    }

    private string GetCombatName(string combatStart)
    {
        var data = combatStart.Split("  ")[1];
        var name = data.Split(',')[2];
        var clearName = name.Trim('"');

        return clearName;
    }

    private int GetDifficulty(string combatStart)
    {
        var data = combatStart.Split("  ")[1];
        var difficulty = data.Split(',')[3];
        
        if (int.TryParse(difficulty, out var diff))
        {
            return diff;
        }

        return 0;
    }

    private int GetDungeonSize(string combatStart)
    {
        var data = combatStart.Split("  ")[1];
        var capacity = data.Split(',')[4];

        if (int.TryParse(capacity, out var cap))
        {
            return cap;
        }

        return 0;
    }

    private bool GetCombatResult(string combatFinish)
    {
        var data = combatFinish.Split("  ");
        var split = data[1].Split(',');
        var combatResult = int.Parse(split[split.Length - 1]);
        var isWin = combatResult == 1;

        return isWin;
    }

    private DateTimeOffset GetTime(string combatStart)
    {
        var parse = combatStart.Split("  ")[0];
        var combatDate = parse.Split(' ');
        var dateWithoutTime = combatDate[0].Split('/');
        var time = combatDate[1].Split('.')[0];
        var clearDate = $"{dateWithoutTime[0]}/{dateWithoutTime[1]}/{DateTimeOffset.UtcNow.Year} {time}";

        DateTimeOffset.TryParse(clearDate, CultureInfo.GetCultureInfo("en-EN"), DateTimeStyles.AssumeUniversal, out var date);

        return date.UtcDateTime;
    }

    private void GetCombatPlayersData(Combat combat)
    {
        var players = GetCombatPlayers(combat.Data, combat);
        combat.Players = players;
    }

    private void CalculatingCommonCombatDetails(Combat combat)
    {
        foreach (var item in combat.Players)
        {
            combat.DamageDone += item.DamageDone;
            combat.HealDone += item.HealDone;
            combat.DamageTaken += item.DamageTaken;
            combat.EnergyRecovery += item.EnergyRecovery;
        }
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

        NotifyObservers();

        _combatNumber++;
    }

    private List<CombatPlayer> GetCombatPlayers(List<string> combatInformation, Combat combat)
    {
        var playersIdAndStats = new Dictionary<string, string>();
        for (var i = 1; i < combatInformation.Count && combatInformation[i].Contains(CombatLogKeyWords.CombatantInfo); i++)
        {
            var combatantInfo = combatInformation[i].Split("  ")[1];
            var playerCombatantInfoArray = combatantInfo.Split(',');
            playersIdAndStats.Add(playerCombatantInfoArray[1], combatantInfo);
        }

        var players = new List<CombatPlayer>();
        foreach (var item in playersIdAndStats)
        {
            var playerCombatantInfoArray = item.Value.Split(new char[2] { '[', ']' });

            var username = GetCombatPlayerUsernameById(combatInformation, item.Key);
            var averageItemLevel = GetAverageItemLevel(playerCombatantInfoArray[1]);
            int usedBuffs = GetUsedBuffs(playerCombatantInfoArray[3]);
            var stats = GetPlayerStats(item.Value);

            var combatDetailsDamageDone = new CombatDetailsDamageDone(_logger);
            var combatDetailsHealDone = new CombatDetailsHealDone(_logger);
            var combatDetailsDamageTaken = new CombatDetailsDamageTaken(_logger);
            var combatDetailsResourceRecovery = new CombatDetailsResourceRecovery(_logger);

            var combatPlayerData = new CombatPlayer
            {
                UserName = username,
                AverageItemLevel = averageItemLevel,
                EnergyRecovery = combatDetailsResourceRecovery.GetData(item.Key, combat.Data),
                DamageDone = combatDetailsDamageDone.GetData(item.Key, combat.Data),
                HealDone = combatDetailsHealDone.GetData(item.Key, combat.Data),
                DamageTaken = combatDetailsDamageTaken.GetData(item.Key, combat.Data),
                UsedBuffs = usedBuffs,
                Stats = stats,
            };

            players.Add(combatPlayerData);
        }

        return players;
    }

    private void GetDungeonName(string combatLog)
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

    private string GetCombatPlayerUsernameById(List<string> combatInformation, string playerId)
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

    private int GetUsedBuffs(string buffsInformation)
    {
        var splitInformations = buffsInformation.Split(",");
        var countOfBuffs = splitInformations.Length / 2;

        return countOfBuffs;
    }

    private double GetAverageItemLevel(string equipmentsInformation)
    {
        var splitEquipementsInformation = equipmentsInformation.Split("))");
        splitEquipementsInformation = splitEquipementsInformation.Select(x => x.TrimStart(',')).ToArray();
        splitEquipementsInformation = splitEquipementsInformation.Select(x => x.TrimStart('(')).ToArray();
        splitEquipementsInformation = splitEquipementsInformation.Select(x => x.Insert(x.Length, ")")).ToArray();

        var ilvl = new List<int>();
        for (int i = 0; i < splitEquipementsInformation.Length - 1; i++)
        {
            var equipmentIlvlInformation = splitEquipementsInformation[i].Split(',')[1];
            int.TryParse(equipmentIlvlInformation, out var equipmentIlvl);
            ilvl.Add(equipmentIlvl);
        }

        var averageILvl = ilvl.Average();
        return averageILvl;
    }

    private PlayerStats GetPlayerStats(string playerCombatantInfo)
    {
        var playerCombatInformation = playerCombatantInfo.Split(',');

        var playerStats = new PlayerStats
        {
            Strength = int.Parse(playerCombatInformation[3]),
            Agility = int.Parse(playerCombatInformation[4]),
            Stamina = int.Parse(playerCombatInformation[5]),
            Intelligence = int.Parse(playerCombatInformation[6]),
            Spirit = int.Parse(playerCombatInformation[7]),
            Dodge = int.Parse(playerCombatInformation[8]),
            Parry = int.Parse(playerCombatInformation[9]),
            CritMelee = int.Parse(playerCombatInformation[11]),
            CritRanged = int.Parse(playerCombatInformation[12]),
            CritSpell = int.Parse(playerCombatInformation[13]),
            HasteMelee = int.Parse(playerCombatInformation[14]),
            HasteRanged = int.Parse(playerCombatInformation[15]),
            HasteSpell = int.Parse(playerCombatInformation[16]),
            HitMelee = int.Parse(playerCombatInformation[17]),
            HitRanged = int.Parse(playerCombatInformation[18]),
            HitSpell = int.Parse(playerCombatInformation[19]),
            Mastery = int.Parse(playerCombatInformation[20]),
            Armor = int.Parse(playerCombatInformation[23]),
        };

        return playerStats;
    }

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update(Combats[^1]);
        }
    }
}
