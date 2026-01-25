namespace CombatParser.Domain.Entities;

public record CombatPlayerDeath
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

    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string LastHitSpell { get; set; } = string.Empty;

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
