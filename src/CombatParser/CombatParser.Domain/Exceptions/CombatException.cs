using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Exceptions;

public class CombatException(string message) : DomainException(message)
{
    public string Message { get; } = message;

    public static void ThrowIfLong(string dungeonName)
    {
        if (dungeonName.Length > Combat.DUNGEON_NAME_MAX_LENGTH)
        {
            throw new CombatLogException("Combat dungeon name length is too long.");
        }
    }

    public static void ThrowIfDateIncorrect(DateTimeOffset startDate, DateTimeOffset finishDate)
    {
        if (startDate > finishDate)
        {
            throw new CombatLogException("Combat start date must be less than finish date.");
        }
    }
}
