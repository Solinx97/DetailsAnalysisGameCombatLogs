using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Entities;

public class CombatTarget
{
    public const int USERNAME_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private CombatTarget() { }

    public CombatTarget(string username, string target, int sum, int combatId)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(sum, nameof(sum));

        Username = username;
        Target = target;
        Sum = sum;
        CombatId = combatId;
    }

    public int Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string Target { get; private set; } = string.Empty;

    public int Sum { get; private set; }

    public Combat Combat { get; private set; }

    public int CombatId { get; private set; }

    public void SetCombatId(int combatId)
    {
        CombatId = combatId;
    }
}
