using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Entities.WoWMidnight;
using CombatAnalysis.WoW.CombatParser.Extensions;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW_12_1_0.CombatParser.Details;
using CombatAnalysis.WoW_12_1_0.CombatParser.Extensions;
using CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Services;

internal class CombatParserService(IFileManager fileManager, ILogger<CombatParserService> logger, IHttpClientHelper httpHelper) : ICombatParserService
{
    private readonly IFileManager _fileManager = fileManager;
    private readonly ILogger<CombatParserService> _logger = logger;
    private readonly IHttpClientHelper _httpHelper = httpHelper;

    private List<PlaceInformation> _zones = [];

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
                var lines = await _fileManager.ReadAllLinesAsync(path, cancellationToken);
                await ProcessCombatLogLinesAsync(lines, petsId, bossCombatStarted, newCombatFromLogs, cancellationToken);
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Request was canceled by client: {Message}", ex.Message);
            Clear();
        }
    }

    public void Clear()
    {
        Combats = [];
        CombatDetails = [];
        _zones = [];
    }

    private async Task ProcessCombatLogLinesAsync(string[] lines, Dictionary<string, List<string>> petsId, bool combatStarted, StringBuilder newCombatFromLogs, CancellationToken cancellationToken)
    {
        foreach (var line in lines)
        {
            combatStarted = await ProcessLine(line, newCombatFromLogs, combatStarted, petsId);
            cancellationToken.ThrowIfCancellationRequested();
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
            var combatInformations = newCombatFromLogsString.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            await GetCombatInformationAsync(combatInformations, petsId);

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
                petList = [];
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

    private async Task GetCombatInformationAsync(string[] builtCombat, Dictionary<string, List<string>> petsId)
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
        if (duration < CombatLogKeyWords.MinCombatDuration)
        {
            return;
        }

        var combatDetails = new CombatDetails(_logger, combat.PetsId);

        var players = await GetCombatPlayers(combat, combatDetails);
        combat.CombatPlayers = [.. players];

        combat.Units = [.. combatDetails.Units.Values];
        combat.UnitCasts = [.. combatDetails.UnitCasts.Values.SelectMany(x => x)];
        combat.UnitHealths = [.. combatDetails.UnitHealths.Values.SelectMany(x => x)];
        combat.UnitPositions = [.. combatDetails.UnitPositions.Values.SelectMany(x => x)];

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

    private async Task<CombatPlayer[]> GetCombatPlayers(Combat combat, CombatDetails combatDetails)
    {
        var combatInformations = combat.Data
            .Where(info => info.Contains(CombatLogKeyWords.CombatantInfo))
            .ToArray();

        var combatPlayers = new CombatPlayer[combatInformations.Length];
        for (var i = 0; i < combatInformations.Length; i++)
        {
            var combatPlayer = await CreateCombatPlayerAsync(combatInformations[i], combat.Data);
            combatPlayers[i] = combatPlayer;
        }

        var playersId = combatPlayers.Select(x => x.Player.GameId).ToArray();

        combatDetails.Calculate(playersId, combat.Data, combat.StartDate, combat.FinishDate);
        combatDetails.CalculateGeneralData(playersId, combat.Duration);

        CombatDetails.Add(combatDetails);

        foreach (var combatPlayer in combatPlayers)
        {
            FillCombatPlayerData(combatPlayer, combatDetails);
        }

        return combatPlayers;
    }

    private async Task<CombatPlayer> CreateCombatPlayerAsync(string combatInformation, string[] combatData)
    {
        var combatInfoList = combatInformation.Split(',');
        var combatInfoSpecialParams = combatInformation.Split(['[', ']']);
        var equipmentsInformation = combatInfoSpecialParams[1];
        var preAurasInformation = combatInfoSpecialParams[5];

        var averageItemLevel = GetAverageItemLevel(equipmentsInformation);

        var statsInformation = combatInfoList.Skip(3).Take(23).ToArray();
        var stats = GetStats(statsInformation);
        var preAuras = GetPreAuras(preAurasInformation);

        var combatPlayer = new CombatPlayer
        {
            AverageItemLevel = double.Round(averageItemLevel, 2),
            Stats = stats,
            Player = new Player
            {
                GameId = combatInfoList[1],
            },
            PreAuras = preAuras,
        };

        var player = await combatPlayer.Player.LoadAsync(_httpHelper, _logger);

        if (player == null)
        {
            await CreatePlayer(combatData, combatInfoList, combatPlayer);
        }
        else
        {
            combatPlayer.Player = player;
        }

        return combatPlayer;
    }

    private async Task CreatePlayer(string[] combatData, string[] combatInfoList, CombatPlayer combatPlayer)
    {
        var username = GetUsernameByPlayerGameId(combatData, combatInfoList[1]);
        var faction = int.Parse(combatInfoList[2]);

        combatPlayer.Player.Username = username;
        combatPlayer.Player.Faction = faction;

        var unit = await combatPlayer.Player.CreateAsync(_httpHelper, _logger);
        if (unit != null)
        {
            combatPlayer.Player = unit;
        }
    }

    private static void FillCombatPlayerData(CombatPlayer combatPlayer, CombatDetails combatDetails)
    {
        combatPlayer.DamageDoneToBoss = combatDetails.DamageDones[combatPlayer.Player.GameId].Where(x => x.Value.IsTargetBoss).Sum(x => x.Value.Value);
        combatPlayer.DamageDone = combatDetails.DamageDones[combatPlayer.Player.GameId].Sum(x => x.Value.Value);
        combatPlayer.HealDone = combatDetails.HealDones[combatPlayer.Player.GameId].Sum(x => x.Value.Value);
        combatPlayer.DamageTaken = combatDetails.DamageTakens[combatPlayer.Player.GameId].Sum(x => x.Value.Value);
        combatPlayer.ResourcesRecovery = combatDetails.ResourcesRecoveries[combatPlayer.Player.GameId].Sum(x => x.Value.Value);

        combatPlayer.Auras.AddRange(combatDetails.Auras[combatPlayer.Player.GameId]);
        combatPlayer.DamageDones.AddRange(combatDetails.DamageDones[combatPlayer.Player.GameId].Select(x => x.Value));
        combatPlayer.DamageDoneGenerals.AddRange(combatDetails.DamageDoneGenerals[combatPlayer.Player.GameId]);
        combatPlayer.HealDones.AddRange(combatDetails.HealDones[combatPlayer.Player.GameId].Select(x => x.Value));
        combatPlayer.HealDoneGenerals.AddRange(combatDetails.HealDoneGenerals[combatPlayer.Player.GameId]);
        combatPlayer.DamageTakens.AddRange(combatDetails.DamageTakens[combatPlayer.Player.GameId].Select(x => x.Value));
        combatPlayer.DamageTakenGenerals.AddRange(combatDetails.DamageTakenGenerals[combatPlayer.Player.GameId]);
        combatPlayer.ResourceRecoveries.AddRange(combatDetails.ResourcesRecoveries[combatPlayer.Player.GameId].Select(x => x.Value));
        combatPlayer.ResourceRecoveryGenerals.AddRange(combatDetails.ResourcesRecoveryGenerals[combatPlayer.Player.GameId]);

        combatPlayer.CombatPlayerDeathes.AddRange(combatDetails.Deathes[combatPlayer.Player.GameId].Select(x => x.Value));
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

    private static string GetUsernameByPlayerGameId(string[] combatData, string gamePlayerId)
    {
        var username = string.Empty;
        for (var i = 1; i < combatData.Length; i++)
        {
            var data = combatData[i].Split(',');
            if (!combatData[i].Contains(CombatLogKeyWords.CombatantInfo)
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

        var averageILvl = ilvl.Any() ? ilvl.Average() : 1;
        return averageILvl;
    }

    private static WoWMidnightPlayerStats GetStats(string[] combatInfo)
    {
        var stats = new WoWMidnightPlayerStats
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

    private static List<CombatPlayerPreAura> GetPreAuras(string preAurasInformation)
    {
        var allPreAuras = preAurasInformation.Split(',');
        var preAuras = new List<CombatPlayerPreAura>();
        for (var i = 0; i + 2 < allPreAuras.Length; i+= 3)
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
