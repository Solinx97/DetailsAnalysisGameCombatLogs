namespace CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

public interface ICombatPlayerResourceRefs : ICombatUnitRefs
{
    int GameSpellId { get; set; }

    string Spell { get; set; }

    int Value { get; set; }

    int ModificationType { get; set; }
}
