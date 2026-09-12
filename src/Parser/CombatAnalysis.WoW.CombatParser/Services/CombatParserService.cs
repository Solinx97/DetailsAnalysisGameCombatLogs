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

public abstract class CombatParserService(ICombatParserHelper combatParserHelper, IFileManager fileManager, ILogger<CombatParserService> logger, IHttpClientHelper httpHelper)
{
    protected readonly ICombatParserHelper _combatParserHelper = combatParserHelper;
    private readonly IFileManager _fileManager = fileManager;
    protected readonly ILogger<CombatParserService> _logger = logger;
    protected readonly IHttpClientHelper _httpHelper = httpHelper;

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
            var units = new ConcurrentDictionary<string, CombatUnit>();
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
        }
    }

    public void Clear()
    {
        foreach (var combat in Combats)
        {
            ClearCombat(combat);
        }

        Combats.Clear();

        foreach (var details in CombatDetails)
        {
            details.Clear();
        }

        CombatDetails.Clear();
        _zones.Clear();

        // Reduce capacity, provided to collections but not release after cleaning collection yet
        Combats.TrimExcess();
        CombatDetails.TrimExcess();
        _zones.TrimExcess();

        // Call GC to collect and release LOH right now
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);
    }

    private async Task ProcessCombatLogLinesAsync(string[] lines, ConcurrentDictionary<string, CombatUnit> units, bool combatStarted, StringBuilder newCombatFromLogs, CancellationToken cancellationToken)
    {
        foreach (var line in lines)
        {
            combatStarted = await ProcessLine(line, newCombatFromLogs, combatStarted, units);
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private async Task<bool> ProcessLine(string line, StringBuilder combatData, bool combatStarted, ConcurrentDictionary<string, CombatUnit> units)
    {
        if (line.Contains(CombatLogKeyWords.SpellSummon))
        {
            var combatDataLine = _combatParserHelper.SplitCombatData(line);
            _combatParserHelper.ParseUnits(units, combatDataLine[6], combatDataLine[7], combatDataLine[8], combatDataLine[2]);
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

    protected static Combat? CreateCombat(string[] builtCombat)
    {
        if (!builtCombat[^1].Contains(CombatLogKeyWords.EncounterEnd))
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

    protected abstract Task GetCombatInformationAsync(string[] builtCombat, ConcurrentDictionary<string, CombatUnit> units);

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

    protected static bool GetCombatResult(string combatFinish)
    {
        var data = combatFinish.Split("  ");
        var split = data[1].Split(',');
        var combatResult = int.Parse(split[split.Length - 1]);
        var isWin = combatResult == 1;

        return isWin;
    }

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
        var players = combat.CombatPlayers;

        combat.DamageDone = players.Sum(player => player.DamageDone);
        combat.HealDone = players.Sum(player => player.HealDone);
        combat.DamageTaken = players.Sum(player => player.DamageTaken);
        combat.ResourcesRecovery = players.Sum(player => player.ResourcesRecovery);
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
            .Where(info => info.Contains(CombatLogKeyWords.CombatantInfo))
            .ToArray();

        var combatPlayers = new CombatPlayer[combatInformations.Length];
        for (var i = 0; i < combatInformations.Length; i++)
        {
            var combatPlayer = await CreateCombatPlayerAsync(combatInformations[i], data);
            combatPlayers[i] = combatPlayer;
        }

        var playersId = combatPlayers.Select(x => x.Player.GameId).ToArray();

        combatDetails.Calculate(playersId, data, start, finish);
        combatDetails.CalculateGeneralData(playersId, duration);

        CombatDetails.Add(combatDetails);

        foreach (var combatPlayer in combatPlayers)
        {
            FillCombatPlayerData(combatPlayer, combatDetails);
        }

        return combatPlayers;
    }

    protected abstract Task<CombatPlayer> CreateCombatPlayerAsync(string combatInformation, string[] combatData);

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

    private static void FillCombatPlayerData(CombatPlayer combatPlayer, CombatDetails combatDetails)
    {
        if (combatDetails.DamageDones.TryGetValue(combatPlayer.Player.GameId, out var damageCollection))
        {
            combatPlayer.DamageDone = damageCollection.Sum(x => x.Value.Value);
            combatPlayer.DamageDones.AddRange(damageCollection.Select(x => x.Value));
        }
        if (combatDetails.DamageTakens.TryGetValue(combatPlayer.Player.GameId, out var damageTakenCollection))
        {
            combatPlayer.DamageTaken = damageTakenCollection.Sum(x => x.Value.Value);
            combatPlayer.DamageDones.AddRange(damageTakenCollection.Select(x => x.Value));
        }

        combatPlayer.HealDone = combatDetails.HealDones[combatPlayer.Player.GameId].Sum(x => x.Value.Value);
        combatPlayer.ResourcesRecovery = combatDetails.ResourcesRecoveries[combatPlayer.Player.GameId].Sum(x => x.Value.Value);

        combatPlayer.Auras.AddRange(combatDetails.Auras[combatPlayer.Player.GameId]);
        if (combatDetails.DamageDoneGenerals.TryGetValue(combatPlayer.Player.GameId, out var damageGeneralCollection))
        {
            combatPlayer.DamageDoneGenerals.AddRange(damageGeneralCollection);
        }
        if (combatDetails.DamageTakenGenerals.TryGetValue(combatPlayer.Player.GameId, out var damageTakenGeneralCollection))
        {
            combatPlayer.DamageDoneGenerals.AddRange(damageTakenGeneralCollection);
        }

        combatPlayer.HealDones.AddRange(combatDetails.HealDones[combatPlayer.Player.GameId].Select(x => x.Value));
        combatPlayer.HealDoneGenerals.AddRange(combatDetails.HealDoneGenerals[combatPlayer.Player.GameId]);
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

    protected virtual List<CombatPlayerPreAura> GetPreAuras(string preAurasInformation)
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

    protected async Task<CombatPlayer> CreateCombatPlayerAsync(string[] statsInformation, string[] combatData, string[] combatInfoList, string preAurasInformation, string equipmentsInformation)
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

    private static void ClearCombat(Combat combat)
    {
        foreach (var player in combat.CombatPlayers)
        {
            player.Auras.Clear();
            player.DamageDones.Clear();
            player.DamageDoneGenerals.Clear();
            player.HealDones.Clear();
            player.HealDoneGenerals.Clear();
            player.ResourceRecoveries.Clear();
            player.ResourceRecoveryGenerals.Clear();
            player.CombatPlayerDeathes.Clear();
            player.PreAuras.Clear();
        }

        foreach (var unit in combat.Units)
        {
            unit.UnitCasts.Clear();
            unit.UnitPositions.Clear();
        }

        combat.CombatPlayers.Clear();
        combat.Units.Clear();
    }
}
