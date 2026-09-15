using CombatParser.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatParser.Domain.Entities.Base;

public class CombatPlayerUnitDataBase : CombatUnitDataBase, IUnitTargetRefs
{
    public string TargetId { get; protected set; }

    public Unit Target { get; protected set; }

    [NotMapped]
    public string TargetGameId { get; protected set; }

    public void SetTargetUnitId(string targetId)
    {
        TargetId = targetId;
    }
}
