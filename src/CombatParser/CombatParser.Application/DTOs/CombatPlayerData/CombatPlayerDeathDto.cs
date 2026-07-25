namespace CombatParser.Application.DTOs.CombatPlayerData;

public class CombatPlayerDeathDto
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string LastHitSpell { get; set; }

    public int LastHitValue { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }
}
