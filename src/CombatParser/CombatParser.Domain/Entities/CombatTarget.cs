using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities;

public class CombatTarget
{
    public int Id { get; set; }

    [MaxLength(126)]
    public string Username { get; set; } = string.Empty;

    [MaxLength(126)]
    public string Target { get; set; } = string.Empty;

    public int Sum { get; set; }

    public Combat Combat { get; set; }

    public int CombatId { get; set; }
}
