namespace CombatParser.Domain.Aggregates;

public class BossMap
{
    public const int NAME_MAX_LENGTH = 128;

    private readonly List<Boss> _bosses = [];

    private BossMap() { }

    public BossMap(int id, int gameId, string name, double x0, double x1, double y0, double y1)
    {
        Id = id;
        GameId = gameId;
        Name = name;
        X0 = x0;
        X1 = x1;
        Y0 = y0;
        Y1 = y1;
    }

    public int Id { get; private set; }

    public int GameId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public double X0 { get; private set; }

    public double X1 { get; private set; }

    public double Y0 { get; private set; }

    public double Y1 { get; private set; }

    public IReadOnlyCollection<Boss> Bosses => _bosses.AsReadOnly();

    public static BossMap Create(int id, int gameId, string name, double x0, double x1, double y0, double y1)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        return new BossMap(id, gameId, name, x0, x1, y0, y1);
    }
}
