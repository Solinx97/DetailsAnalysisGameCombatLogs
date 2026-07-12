namespace CombatParser.Domain.Aggregates;

public class Boss
{
    public const int NAME_MAX_LENGTH = 128;

    private Boss() { }

    private Boss(int id, int gameId, string name, long health, int difficult, int size, int bossMapId)
    {
        Id = id;
        GameId = gameId;
        Name = name;
        Health = health;
        Difficult = difficult;
        Size = size;
        BossMapId = bossMapId;
    }

    public int Id { get; private set; }

    public int GameId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public long Health { get; private set; }

    public int Difficult { get; private set; }

    public int Size { get; private set; }

    public BossMap BossMap { get; private set; }

    public int BossMapId { get; private set; }

    public ICollection<BestSpecializationScore> BestSpecializationScores { get; private set; } = [];

    public static Boss Create(int id, int gameId, string name, long health, int difficult, int size, int bossMapId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegative(health, nameof(health));
        ArgumentOutOfRangeException.ThrowIfNegative(difficult, nameof(difficult));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bossMapId, nameof(bossMapId));

        return new Boss(id, gameId, name, health, difficult, size, bossMapId);
    }
}
