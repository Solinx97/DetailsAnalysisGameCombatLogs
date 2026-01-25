namespace CombatParser.Domain.Entities;

public class CombatPlayerDeath
{
    public const int USERNAME_MAX_LENGTH = 128;
    public const int SPELL_MAX_LENGTH = 128;

    private CombatPlayerDeath() { }

    public CombatPlayerDeath(string username, string lastHitSpell, int lastHitValue, TimeSpan time, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
        ArgumentOutOfRangeException.ThrowIfNegative(lastHitValue, nameof(lastHitValue));

        Username = username;
        LastHitSpell = lastHitSpell;
        LastHitValue = lastHitValue;
        Time = time;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string LastHitSpell { get; private set; } = string.Empty;

    public int LastHitValue { get; private set; }

    public TimeSpan Time { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public int CombatPlayerId { get; private set; }

    public void SetCombatPlayerId(int combatPlayerId)
    {
        CombatPlayerId = combatPlayerId;
    }
}
