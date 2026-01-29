namespace CombatParser.Domain.Data.Filters;

public interface IGeneralFilterEntity
{
    string Spell { get; }

    int Value { get; }

    TimeSpan Time { get; }

    string Creator { get; }

    string Target { get; }
}
