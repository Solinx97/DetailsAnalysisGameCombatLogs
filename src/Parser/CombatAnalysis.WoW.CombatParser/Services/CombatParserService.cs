using CombatAnalysis.WoW.CombatParser.Core;
using CombatAnalysis.WoW.CombatParser.Details;
using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Extensions;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime;
using System.Text;

namespace CombatAnalysis.WoW.CombatParser.Services;

public abstract class CombatParserService(ICombatParserHelper combatParserHelper, IFileManager fileManager, 
    ILogger<CombatParserService> logger, IHttpClientHelper httpHelper)
{
    protected readonly ICombatParserHelper _combatParserHelper = combatParserHelper;
    protected readonly IFileManager _fileManager = fileManager;
    protected readonly ILogger<CombatParserService> _logger = logger;
    protected readonly IHttpClientHelper _httpHelper = httpHelper;

    private readonly List<PlaceInformation> _zones = [];

    protected abstract string LogBuildVersion { get; set; }

    public List<Combat> Combats { get; private set; } = [];

    public async Task<bool> FileCheckAsync(string combatLog)
    {
        using var reader = _fileManager.StreamReader(combatLog);
        var line = await reader.ReadLineAsync();
        if (line == null)
        {
            return false;
        }

        var isCombatLogFile = line.Contains(CombatLogKeyWords.COMBAT_LOG_VERSION);
        if (!isCombatLogFile)
        {
            return false;
        }

        var split = line.Split("  ")[1].Split(',');
        var build = split[5].Split('.');
        var isReleventVersion = build[0].Equals(LogBuildVersion);

        return isReleventVersion;
    }

    public async Task ParseAsync(List<string> combatLogPaths, CancellationToken cancellationToken)
    {
        try
        {
            var newCombatFromLogs = new StringBuilder();
            var units = new ConcurrentDictionary<string, Unit>();
            var bossCombatStarted = false;

            Clear();

            foreach (var path in combatLogPaths)
            {
                var lines = await _fileManager.ReadAllLinesAsync(path, cancellationToken);
                await ProcessCombatLogLinesAsync(lines, units, bossCombatStarted, newCombatFromLogs, cancellationToken);
            }
        }
        catch (OperationCanceledException ex)
        {
            logger.LogError(ex, "Request was canceled by client: {Message}", ex.Message);
            Clear();

            throw;
        }
    }

    public void Clear()
    {
        foreach (var combat in Combats)
        {
            ClearCombat(combat);
        }

        Combats.Clear();
        _zones.Clear();

        // Reduce capacity, provided to collections but not release after cleaning collection yet
        Combats.TrimExcess();
        _zones.TrimExcess();

        // Call GC to collect and release LOH right now
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);
    }

    private async Task ProcessCombatLogLinesAsync(string[] lines, ConcurrentDictionary<string, Unit> units, bool combatStarted, StringBuilder newCombatFromLogs, CancellationToken cancellationToken)
    {
        foreach (var line in lines)
        {
            combatStarted = await ProcessLine(line, newCombatFromLogs, combatStarted, units);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private async Task<bool> ProcessLine(string line, StringBuilder combatData, bool combatStarted, ConcurrentDictionary<string, Unit> units)
    {
        if (line.Contains(CombatLogKeyWords.SPELL_SUMMON))
        {
            var combatDataLine = _combatParserHelper.SplitCombatData(line);
            _combatParserHelper.ParseUnits(units, combatDataLine[6], combatDataLine[7], combatDataLine[8], combatDataLine[2]);
        }
        
        if (line.Contains(CombatLogKeyWords.ZONE_CHANGE))
        {
            ZoneName(line);
        }
       
        if (line.Contains(CombatLogKeyWords.ENCOUNTER_START))
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

        if (line.Contains(CombatLogKeyWords.ENCOUNTER_END))
        {
            combatStarted = false;

            combatData.AppendLine(line);

            var newCombatFromLogsString = combatData.ToString();
            var combatInformations = newCombatFromLogsString.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            await GetCombatInformationAsync(combatInformations, units);

            combatData.Clear();
            combatData.Capacity = 16;
            units.Clear();
        }
        else
        {
            combatData.AppendLine(line);
        }

        return combatStarted;
    }

    protected Combat? CreateCombat(string[] builtCombat)
    {
        if (!builtCombat[^1].Contains(CombatLogKeyWords.ENCOUNTER_END))
        {
            return null;
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
            IsWin = GetCombatResult(builtCombat[^1]),
            StartDate = GetTime(builtCombat[0]),
            FinishDate = GetTime(builtCombat[^1]),
        };

        return combat;
    }
    
    private async Task GetCombatInformationAsync(string[] builtCombat, ConcurrentDictionary<string, Unit> units)
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

        var combatDetails = GetCombatDetails(units);

        var players = await GetCombatPlayers(builtCombat, combat.Duration, combat.StartDate, combat.FinishDate, combatDetails);
        combat.CombatPlayers = [.. players];

        combat.Units = [.. combatDetails.Units.Values];

        CalculatingCommonCombatDetails(combat);

        AddNewCombat(combat);
    }

    protected abstract CombatDetails GetCombatDetails(ConcurrentDictionary<string, Unit> units);

    protected static int GetGameBossId(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var gameBossId = data.Split(',')[1];
        var convertToInt = Convert.ToInt32(gameBossId);

        return convertToInt;
    }

    protected static int GetDifficulty(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var difficulty = data.Split(',')[3];
        var convertToInt = Convert.ToInt32(difficulty);

        return convertToInt;
    }

    protected static int GetGroupSize(string encounterStart)
    {
        var data = encounterStart.Split("  ")[1];
        var groupSize = data.Split(',')[4];
        var convertToInt = Convert.ToInt32(groupSize);

        return convertToInt;
    }

    protected abstract bool GetCombatResult(string combatFinish);

    protected static DateTimeOffset GetTime(string combatStart)
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

    protected static void CalculatingCommonCombatDetails(Combat combat)
    {
        var players = combat.Units
            .Where(x => x.Type == (int)CombatUnitType.Player)
            .ToList();

        combat.DamageDone = players.Sum(player => player.UnitInfo.DamageDone);
        combat.HealDone = players.Sum(player => player.UnitInfo.HealDone);
        combat.DamageTaken = players.Sum(player => player.UnitInfo.DamageTaken);
        combat.ResourcesRecovery = players.Sum(player => player.UnitInfo.ResourcesRecovery);
    }

    protected void AddNewCombat(Combat combat)
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

    protected async Task<CombatPlayer[]> GetCombatPlayers(string[] data, string duration, DateTimeOffset start, DateTimeOffset finish, CombatDetails combatDetails)
    {
        var combatInformations = data
            .Where(info => info.Contains(CombatLogKeyWords.COMBATANT_INFO))
            .ToArray();

        combatDetails.Calculate(data, start, finish);

        var combatPlayers = new CombatPlayer[combatInformations.Length];
        for (var i = 0; i < combatInformations.Length; i++)
        {
            var combatPlayer = await GetCombatPlayerDataAsync(combatInformations[i], data, combatDetails.Units);
            combatPlayers[i] = combatPlayer;
        }

        var playersId = combatPlayers.Select(x => x.Player.GameId).ToArray();
        foreach (var combatPlayerUnit in combatDetails.Units.Values)
        {
            ApplyUnitInfo(combatPlayerUnit, combatDetails.Units);
        }

        return combatPlayers;
    }

    protected abstract Task<CombatPlayer> GetCombatPlayerDataAsync(string combatInformation, string[] combatData, ConcurrentDictionary<string, Unit> units);

    protected async Task CreatePlayer(string[] combatData, string[] combatInfoList, CombatPlayer combatPlayer)
    {
        var username = GetUsernameByPlayerGameId(combatData, combatInfoList[1]);
        var faction = int.Parse(combatInfoList[2]);

        combatPlayer.Player.Username = username;
        combatPlayer.Player.Faction = faction;

        var player = await combatPlayer.Player.CreateAsync(_httpHelper, _logger);
        if (player != null)
        {
            combatPlayer.Player = player;
        }
    }

    private static void ApplyUnitInfo(Unit unit, ConcurrentDictionary<string, Unit> units)
    {
        UnitInfo unitInfo;
        if (unit.CreatorGameId != null && units.TryGetValue(unit.CreatorGameId, out var creatorUnit))
        {
            unitInfo = creatorUnit.UnitInfo;
        }
        else
        {
            unitInfo = unit.UnitInfo;
        }

        unitInfo.DamageDone += unit.DamageDones.Sum(x => x.Value);
        unitInfo.DamageTaken += unit.DamageTakens.Sum(x => x.Value);
        unitInfo.HealDone += unit.HealDones.Sum(x => x.Value - x.Overheal);
        unitInfo.ResourcesRecovery += unit.ResourceRecoveries.Sum(x => x.Value);
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
            if (!combatData[i].Contains(CombatLogKeyWords.COMBATANT_INFO)
                && gamePlayerId == data[1])
            {
                var dirtyUsername = data[2];
                username = dirtyUsername.Trim('"');
                break;
            }
        }

        return username;
    }

    protected static double GetAverageItemLevel(string equipmentsInformation)
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

    protected abstract IPlayerStats GetStats(string[] combatInfo);

    protected virtual List<UnitPreAura> GetPreAuras(string preAurasInformation)
    {
        var allPreAuras = preAurasInformation.Split(',');
        var preAuras = new List<UnitPreAura>();

        for (var i = 0; i + 2 < allPreAuras.Length; i += 3)
        {
            var preAura = new UnitPreAura
            {
                TargetGameId = allPreAuras[i],
                GameId = int.Parse(allPreAuras[i + 1]),
                Status = int.Parse(allPreAuras[i + 2]),
            };
            preAuras.Add(preAura);
        }

        return preAuras;
    }

    protected async Task<CombatPlayer> CreateCombatPlayerAsync(string[] statsInformation, string[] combatData, string[] combatInfoList, string preAurasInformation, string equipmentsInformation, ConcurrentDictionary<string, Unit> units)
    {
        var averageItemLevel = GetAverageItemLevel(equipmentsInformation);

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
            UnitGameId = combatInfoList[1],
        };

        if (units.TryGetValue(combatPlayer.UnitGameId, out var unit))
        {
            unit.PreAuras.AddRange(preAuras);
        }

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

    private static void ClearCombat(Combat combat)
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
