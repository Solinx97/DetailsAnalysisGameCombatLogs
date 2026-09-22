using CombatParser.Domain.Entities;
using CombatParser.Domain.Exceptions;

namespace CombatParser.Domain.Aggregates;

public class CombatLog
{
    public const int NAME_MAX_LENGTH = 128;

    private List<CombatLogStatus> _statuses = [];
    private List<Combat> _combats = [];

    private CombatLog() { }

    private CombatLog(int gameVersion, string name, int logType, string appUserId)
    {
        GameVersion = gameVersion;
        Name = name;
        Date = DateTimeOffset.UtcNow;
        LogType = logType;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public int GameVersion { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset Date { get; private set; }

    public int LogType { get; private set; }

    public string AppUserId { get; private set; }

    public IReadOnlyCollection<CombatLogStatus> Statuses => _statuses.AsReadOnly();

    public IReadOnlyCollection<Combat> Combats => _combats.AsReadOnly();

    public static CombatLog Create(int gameVersion, string name, int logType, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentOutOfRangeException.ThrowIfNegative(logType, nameof(logType));

        CombatLogException.ThrowIfLong(name);

        return new CombatLog(gameVersion, name, logType, appUserId);
    }

    public void AddStatus(int status)
    {
        var combatLogStatus = CombatLogStatus.Create(status);
        _statuses.Add(combatLogStatus);
    }

    public void Edit(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        CombatLogException.ThrowIfLong(name);

        if (!string.Equals(name, Name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
        }
    }
}