namespace CombatAnalysis.DAL.Interfaces.Entities;

public interface ICombatPlayerEntity : IEntity
{
    int CombatPlayerId { get; set; }
}