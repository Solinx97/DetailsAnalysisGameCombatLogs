namespace CombatParser.Domain.Data;

public interface IGeneralEntity
{
    string Spell { get; }

    int Value { get; }

    TimeSpan Time { get; }

    string Creator { get; }

    string Target { get; }
}
