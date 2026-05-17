using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Entities;

public class Boss
{
    public const int NAME_MAX_LENGTH = 128;

    private Boss() { }

    public Boss(int id, int gameId, string name, long health, int difficult, int size)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gameId, nameof(gameId));
        ArgumentOutOfRangeException.ThrowIfNegative(health, nameof(health));

        Id = id;
        GameId = gameId;
        Name = name;
        Health = health;
        Difficult = difficult;
        Size = size;
    }

    public int Id { get; private set; }

    public int GameId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public long Health { get; private set; }

    public int Difficult { get; private set; }

    public int Size { get; private set; }

    public ICollection<BestSpecializationScore> BestSpecializationScores { get; private set; } = [];
}
