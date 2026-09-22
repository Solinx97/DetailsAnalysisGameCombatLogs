namespace CombatParser.Domain.Entities;

public class UnitInfo
{
    private UnitInfo() { }

    private UnitInfo(int resourcesRecovery, int damageDone, int healDone, int damageTaken)
    {
        Id = Guid.NewGuid().ToString();
        ResourcesRecovery = resourcesRecovery;
        DamageDone = damageDone;
        HealDone = healDone;
        DamageTaken = damageTaken;
    }

    public string Id { get; protected set; }

    public long ResourcesRecovery { get; private set; }

    public long DamageDone { get; private set; }

    public long HealDone { get; private set; }

    public long DamageTaken { get; private set; }

    public string UnitId { get; private set; }

    public void SetUnitId(string unitId)
    {
        UnitId = unitId;
    }

    public static UnitInfo Create(int resourcesRecovery, int damageDone, int healDone, int damageTaken)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(damageDone, nameof(damageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(healDone, nameof(healDone));

        return new UnitInfo(resourcesRecovery, damageDone, healDone, damageTaken);
    }
}
