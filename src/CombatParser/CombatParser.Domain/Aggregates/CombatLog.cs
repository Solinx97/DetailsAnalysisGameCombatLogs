using CombatParser.Domain.Exceptions;

namespace CombatParser.Domain.Aggregates;

public class CombatLog
{
    public const int NAME_MAX_LENGTH = 128;

    private CombatLog() { }

    private CombatLog(string name, int logType, int numberReadyCombats, int combatsInQueue, string appUserId)
    {
        Name = name;
        Date = DateTimeOffset.UtcNow;
        LogType = logType;
        NumberReadyCombats = numberReadyCombats;
        CombatsInQueue = combatsInQueue;
        IsReady = false;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public DateTimeOffset Date { get; private set; }

    public int LogType { get; private set; }

    public int NumberReadyCombats { get; private set; }

    public int CombatsInQueue { get; private set; }

    public bool IsReady { get; private set; }

    public string AppUserId { get; private set; } = string.Empty;

    public ICollection<Combat> Combats { get; set; } = [];

    public static CombatLog Create(string name, int logType, int numberReadyCombats, int combatsInQueue, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentOutOfRangeException.ThrowIfNegative(logType, nameof(logType));
        ArgumentOutOfRangeException.ThrowIfNegative(numberReadyCombats, nameof(numberReadyCombats));
        ArgumentOutOfRangeException.ThrowIfNegative(combatsInQueue, nameof(combatsInQueue));

        CombatLogException.ThrowIfLong(name);

        return new CombatLog(name, logType, numberReadyCombats, combatsInQueue, appUserId);
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

    public void CombatLogIsReady(int numberReadyCombats, int combatsInQueue)
    {
        NumberReadyCombats = numberReadyCombats;
        CombatsInQueue = combatsInQueue;
        IsReady = true;
    }
}