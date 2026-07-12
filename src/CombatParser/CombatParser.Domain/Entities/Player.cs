namespace CombatParser.Domain.Entities;

public class Player
{
    public const int GAMEID_MAX_LENGTH = 128;
    public const int USERNAME_MAX_LENGTH = 128;

    private Player() { }

    public Player(string gameId, string username, int faction)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
        ArgumentOutOfRangeException.ThrowIfNegative(faction, nameof(faction));

        Id = Guid.NewGuid().ToString();
        GameId = gameId;
        Username = username;
        Faction = faction;
    }

    public string Id { get; private set; } = string.Empty;

    public string GameId { get; private set; } = string.Empty;

    public string Username { get; private set; } = string.Empty;

    public int Faction { get; private set; }

    public ICollection<CombatPlayer> CombatPlayers { get; private set; } = [];

    public static Player Create(string gameId, string username, int faction)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
        ArgumentOutOfRangeException.ThrowIfNegative(faction, nameof(faction));

        return new Player(gameId, username, faction);
    }
}
