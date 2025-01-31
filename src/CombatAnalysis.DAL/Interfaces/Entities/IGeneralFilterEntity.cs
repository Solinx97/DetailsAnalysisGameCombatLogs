namespace CombatAnalysis.DAL.Interfaces.Entities;

public interface IGeneralFilterEntity
{
    string Spell { get; set; }

    int Value { get; set; }

    TimeSpan Time { get; set; }

    string Creator { get; set; }

    string Target { get; set; }
}
