using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Interfaces;

public interface IUnitTargetRefs
{
    string TargetId { get; }

    Unit Target { get; }

    string TargetGameId { get; }

    void SetTargetUnitId(string targetId);
}
