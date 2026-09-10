using CombatParser.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatParser.Domain.Entities.Base;

public class CombatUnitDataBase : CombatPlayerDataBase, ICombatUnitRefs
{
    public string CreatorId { get; protected set; }

    [NotMapped]
    public string CreatorGameId { get; protected set; }

    public string TargetId { get; protected set; }

    [NotMapped]
    public string TargetGameId { get; protected set; }

    public void SetUnits(string creatorId, string targetId)
    {
        CreatorId = creatorId;
        TargetId = targetId;
    }
}
