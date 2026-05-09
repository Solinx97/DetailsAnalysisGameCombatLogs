namespace CombatParser.Domain.Entities;

public class CombatDataBase
{
    public int CombatId { get; protected set; }

    public void SetCombatId(int combatId)
    {
        CombatId = combatId;
    }
}
