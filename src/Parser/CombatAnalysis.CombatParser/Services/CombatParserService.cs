using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParser.Extensions;
using CombatAnalysis.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CombatAnalysis.CombatParser.Services;

internal class CombatParserService(IFileManager fileManager, ILogger logger, IHttpClientHelper httpHelper) : ICombatParserService
{
    private readonly IList<PlaceInformation> _zones = [];
    private readonly IFileManager _fileManager = fileManager;
    private readonly ILogger _logger = logger;
    private readonly IHttpClientHelper _httpHelper = httpHelper;

    private readonly TimeSpan _minCombatDuration = TimeSpan.Parse("00:00:20");

    public List<Combat> Combats { get; private set; } = [];

    public List<CombatDetails> CombatDetails { get; private set; } = [];

    public async Task<bool> FileCheckAsync(string combatLog)
    {
        using var reader = _fileManager.StreamReader(combatLog);
        var line = await reader.ReadLineAsync();

        var fileIsCorrect = !string.IsNullOrEmpty(line) && line.Contains(CombatLogKeyWords.CombatLogVersion);

        return fileIsCorrect;
    }

    public async Task ParseAsync(List<string> combatLogPaths, CancellationToken cancellationToken)
    {
        try
        {
            var newCombatFromLogs = new StringBuilder();
            var petsId = new Dictionary<string, List<string>>();
            var bossCombatStarted = false;

            Clear();

            foreach (var path in combatLogPaths)
            {
                var lines = await fileManager.ReadAllLinesAsync(path, cancellationToken);
                await ProcessCombatLogLinesAsync(lines, petsId, bossCombatStarted, newCombatFromLogs);
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Request was canceled by client: {Message}", ex.Message);
        }
    }

    public void Clear()
    {
        Combats.Clear();
        _zones.Clear();
    }

    private async Task ProcessCombatLogLinesAsync(string[] lines, Dictionary<string, List<string>> petsId, bool combatStarted, StringBuilder newCombatFromLogs)
    {
        foreach (var line in lines)
        {
            combatStarted = await ProcessLine(line, newCombatFromLogs, combatStarted, petsId);
        }
    }

    private async Task<bool> ProcessLine(string line, StringBuilder combatData, bool combatStarted, Dictionary<string, List<string>> petsId)
    {
        if (line.Contains(CombatLogKeyWords.SpellSummon))
        {
            ParsePlayerCreatures(line, petsId);
        }
        
        if (line.Contains($"{CombatLogKeyWords.SwingDamage},") && line.Contains(CombatLogKeyWords.Pet))
        {
            ParsePlayerPets(line, petsId);
        }
        
        if (line.Contains(CombatLogKeyWords.ZoneChange))
        {
            ZoneName(line);
        }
       
        if (line.Contains(CombatLogKeyWords.EncounterStart))
        {
            // If during combat player can be disconnected, lagged or some bugs, end of combat (encounter_end) can be not writed in log file.
            // If not find end of combat, parsing will continue and get information from next combat as current combat information.
            // Better clean all stored information, if end of combat not be find.
            combatData.Clear();

            combatData.AppendLine(line);

            return true;
        }

        if (!combatStarted)
        {
            return false;
        }

        if (line.Contains(CombatLogKeyWords.EncounterEnd))
        {
            combatStarted = false;

            combatData.AppendLine(line);

            var newCombatFromLogsString = combatData.ToString();
            var combatInformationList = newCombatFromLogsString.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();

            await GetCombatInformationAsync(combatInformationList, petsId);

            combatData.Clear();
            petsId = [];
        }
        else
        {
            combatData.AppendLine(line);
        }

        return combatStarted;
    }

    private static void ParsePlayerCreatures(string data, Dictionary<string, List<string>> creaturesId)
    {
        var splitStr = data.Split("  ")[1].Split(',');
        var playerId = splitStr[1].Contains(CombatLogKeyWords.Player) 
            ? splitStr[1]
            : string.Empty;
        var friendlyCreatureId = splitStr[1].Contains(CombatLogKeyWords.Creature)
            ? splitStr[1]
            : string.Empty;

        if (string.IsNullOrEmpty(playerId) && string.IsNullOrEmpty(friendlyCreatureId))
        {
            return;
        }

        var creatureId = splitStr[5];
        var friendCreaturePlayerId = creaturesId.FirstOrDefault(x => x.Value.Contains(friendlyCreatureId)).Key;
        if (!string.IsNullOrEmpty(friendCreaturePlayerId))
        {
            if (creaturesId.TryGetValue(friendCreaturePlayerId, out var petList))
            {
                petList.Add(creatureId);
            }
        }
        else
        {
            if (!creaturesId.TryGetValue(playerId, out var petList))
            {
                petList = new List<string>();
                creaturesId[playerId] = petList;
            }

            petList.Add(creatureId);
        }
    }

    private static void ParsePlayerPets(string data, Dictionary<string, List<string>> petsId)
    {
        var combatLogParts = data.Split("  ")[1].Split(',');

        if (combatLogParts[3].Contains("0x10a48"))
        {
            return;
        }

        var playerId = combatLogParts[10].Contains(CombatLogKeyWords.Player) ? combatLogParts[10] : string.Empty;

        if (string.IsNullOrEmpty(playerId))
        {
            return;
        }

        var petId = combatLogParts[1];
        if (!petsId.TryGetValue(playerId, out var petList))
        {
            petList = [];
            petsId[playerId] = petList;
        }

        if (!petList.Any(x => x.Equals(petId)))
        {
            petList.Add(petId);
        }
    }

    private async Task GetCombatInformationAsync(List<string> builtCombat, Dictionary<string, List<string>> petsId)
    {
        if (!builtCombat[^1].Contains(CombatLogKeyWords.EncounterEnd))
        {
            return;
        }

        var boss = new Boss
        {
            GameId = GetGameBossId(builtCombat[0]),
            Difficult = GetDifficulty(builtCombat[0]),
            Size = GetGroupSize(builtCombat[0])
        };

        var combat = new Combat
        {
            Boss = boss,
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

        var players = await GetCombatPlayers(combat);
        combat.CombatPlayers = players;

        CalculatingCommonCombatDetails(combat);

        AddNewCombat(combat);
    }

    private static int GetGameBossId(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var gameBossId = data.Split(',')[1];
        var convertToInt = Convert.ToInt32(gameBossId);

        return convertToInt;
    }

    private static int GetDifficulty(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var difficulty = data.Split(',')[3];
        var convertToInt = Convert.ToInt32(difficulty);

        return convertToInt;
    }

    private static int GetGroupSize(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var groupSize = data.Split(',')[4];
        var convertToInt = Convert.ToInt32(groupSize);

        return convertToInt;
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
        string[] formats =
        {
            "M/d/yyyy HH:mm:ss.ffff",
            "MM/dd/yyyy HH:mm:ss.ffff"
        };

        var parse = combatStart.Split("  ")[0];

        if (DateTimeOffset.TryParseExact(parse, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var date))
        {
            return date.UtcDateTime;
        }

        return DateTimeOffset.MinValue;
    }

    private static void CalculatingCommonCombatDetails(Combat combat)
    {
        var players = combat.CombatPlayers;

        combat.DamageDone = players.Sum(player => player.DamageDone);
        combat.HealDone = players.Sum(player => player.HealDone);
        combat.DamageTaken = players.Sum(player => player.DamageTaken);
        combat.ResourcesRecovery = players.Sum(player => player.ResourcesRecovery);
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

        Combats.Add(combat);
    }

    private async Task<List<CombatPlayer>> GetCombatPlayers(Combat combat)
    {
        var combatInformations = combat.Data
            .Where(info => info.Contains(CombatLogKeyWords.CombatantInfo))
            .ToList();

        var combatPlayers = new List<CombatPlayer>();
        foreach (var item in combatInformations)
        {
            var combatInfoList = item.Split(',');
            var combatInfoEquipments = item.Split(['[', ']']);

            var averageItemLevel = GetAverageItemLevel(combatInfoEquipments[1]);

            var statsInformation = combatInfoList.Skip(3).Take(30).ToArray();
            var stats = GetStats(statsInformation);

            var combatPlayer = new CombatPlayer
            {
                AverageItemLevel = double.Round(averageItemLevel, 2),
                Stats = stats,
                Player = new Player
                {
                    GameId = combatInfoList[1],
                },
            };

            var player = await combatPlayer.Player.LoadAsync(_httpHelper, _logger);

            if (player == null)
            {
                var username = GetUsernameByPlayerGameId(combat.Data, combatInfoList[1]);
                var faction = int.Parse(combatInfoList[2]);

                combatPlayer.Player.Id = Guid.NewGuid().ToString();
                combatPlayer.Player.Username = username;
                combatPlayer.Player.Faction = faction;

                var newPlayer = await combatPlayer.Player.CreateAsync(_httpHelper, _logger);
                if (newPlayer != null)
                {
                    combatPlayer.Player = newPlayer;
                }
            }
            else
            {
                combatPlayer.Player = player;
            }

            combatPlayers.Add(combatPlayer);
        }

        var playersId = combatPlayers.Select(x => x.Player.GameId).ToList();

        var combatDetails = new CombatDetails(_logger, combat.PetsId);
        combatDetails.Calculate(playersId, combat.Data, combat.StartDate, combat.FinishDate);
        combatDetails.CalculateGeneralData(playersId, combat.Duration);

        CombatDetails.Add(combatDetails);

        var combatAuras = new List<CombatAura>();
        foreach (var combatPlayer in combatPlayers)
        {
            combatPlayer.DamageDoneToBoss = combatDetails.DamageDone[combatPlayer.Player.GameId].Where(x => x.Value.IsTargetBoss).Sum(x => x.Value.Value);
            combatPlayer.DamageDone = combatDetails.DamageDone[combatPlayer.Player.GameId].Sum(x => x.Value.Value);
            combatPlayer.HealDone = combatDetails.HealDone[combatPlayer.Player.GameId].Sum(x => x.Value.Value);
            combatPlayer.DamageTaken = combatDetails.DamageTaken[combatPlayer.Player.GameId].Sum(x => x.Value.Value);
            combatPlayer.ResourcesRecovery = combatDetails.ResourcesRecovery[combatPlayer.Player.GameId].Sum(x => x.Value.Value);

            combatPlayer.DamageDones.AddRange(combatDetails.DamageDone[combatPlayer.Player.GameId].Select(x => x.Value));
            combatPlayer.DamageDoneGenerals.AddRange(combatDetails.DamageDoneGeneral[combatPlayer.Player.GameId]);
            combatPlayer.HealDones.AddRange(combatDetails.HealDone[combatPlayer.Player.GameId].Select(x => x.Value));
            combatPlayer.HealDoneGenerals.AddRange(combatDetails.HealDoneGeneral[combatPlayer.Player.GameId]);
            combatPlayer.DamageTakens.AddRange(combatDetails.DamageTaken[combatPlayer.Player.GameId].Select(x => x.Value));
            combatPlayer.DamageTakenGenerals.AddRange(combatDetails.DamageTakenGeneral[combatPlayer.Player.GameId]);
            combatPlayer.ResourceRecoveries.AddRange(combatDetails.ResourcesRecovery[combatPlayer.Player.GameId].Select(x => x.Value));
            combatPlayer.ResourceRecoveryGenerals.AddRange(combatDetails.ResourcesRecoveryGeneral[combatPlayer.Player.GameId]);

            combatPlayer.CombatPlayerDeathes.AddRange(combatDetails.CombatPlayersDeath[combatPlayer.Player.GameId].Select(x => x.Value));
            combatPlayer.CombatPlayerPositions.AddRange(combatDetails.CombatPlayerPositions[combatPlayer.Player.GameId].Select(x => x.Value));

            combatAuras.AddRange(combatDetails.Auras[combatPlayer.Player.GameId].Select(x => x.Value));
        }

        combat.CombatAuras = combatAuras;

        return combatPlayers;
    }

    private void ZoneName(string combatLog)
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

    private static string GetUsernameByPlayerGameId(List<string> logData, string gamePlayerId)
    {
        var username = string.Empty;
        for (var i = 1; i < logData.Count; i++)
        {
            var data = logData[i].Split(',');
            if (!logData[i].Contains(CombatLogKeyWords.CombatantInfo)
                && gamePlayerId == data[1])
            {
                var dirtyUsername = data[2];
                username = dirtyUsername.Trim('"');
                break;
            }
        }

        return username;
    }

    private static double GetAverageItemLevel(string equipmentsInformation)
    {
        var splitEquipementsInformation = equipmentsInformation.Split("))");

        var ilvl = new List<int>();
        for (var i = 0; i < splitEquipementsInformation.Length - 2; i++)
        {
            var equipmentIlvlInformation = splitEquipementsInformation[i].Trim(',').Split(',')[1];
            if (int.TryParse(equipmentIlvlInformation, out var equipmentIlvl) && equipmentIlvl > 1)
            {
                ilvl.Add(equipmentIlvl);
            }
        }

        var averageILvl = ilvl.Average();
        return averageILvl;
    }

    private static PlayerStats GetStats(string[] combatInfo)
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
