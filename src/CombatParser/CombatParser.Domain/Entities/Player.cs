using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities;

public class Player
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    [MaxLength(126)]
    public string Username { get; set; } = string.Empty;

    public int Faction { get; set; }

    //public ICollection<CombatPlayer> CombatPlayers { get; set; } = [];
}
