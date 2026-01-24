using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Exceptions;

public class CombatLogException(string message) : DomainException(message)
{
    public string Message { get; } = message;

    public static void ThrowIfLong(string name)
    {
        if (name.Length > CombatLog.NAME_MAX_LENGTH)
        {
            throw new CombatLogException("Combat logs name length is too long.");
        }
    }
}
