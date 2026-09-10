using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface IGeneralEntity
{
    string Spell { get; }

    int Value { get; }

    TimeSpan Time { get; }

    CombatUnit Creator { get; }

    CombatUnit Target { get; }
}
