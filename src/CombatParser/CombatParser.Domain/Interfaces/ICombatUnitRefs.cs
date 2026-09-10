namespace CombatParser.Domain.Interfaces;

public interface ICombatUnitRefs
{
    string CreatorId { get; }

    string CreatorGameId { get; }

    string TargetId { get; }

    string TargetGameId { get; }

    void SetUnits(string creatorId, string targetId);
}
