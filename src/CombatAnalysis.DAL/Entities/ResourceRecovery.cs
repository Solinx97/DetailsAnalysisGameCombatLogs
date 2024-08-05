using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Entities;

[Index(nameof(CombatPlayerId))]
public class ResourceRecovery : BasePlayerInfo
{
    public int Id { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public string SpellOrItem { get; set; }

    public override int CombatPlayerId { get; set; }
}
