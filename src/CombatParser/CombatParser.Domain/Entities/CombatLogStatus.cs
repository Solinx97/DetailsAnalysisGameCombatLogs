using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Entities;

public class CombatLogStatus
{
    private CombatLogStatus() { }

    private CombatLogStatus(int status, DateTimeOffset date)
    {
        Status = status;
        Date = date;
    }

    public int Id { get; private set; }

    public DateTimeOffset Date { get; private set; }

    public int Status { get; private set; }

    public CombatLog CombatLog { get; private set; }

    public int CombatLogId { get; private set; }

    public static CombatLogStatus Create(int status)
    {
        var now = DateTimeOffset.UtcNow;
        return new CombatLogStatus(status, now);
    }
}
