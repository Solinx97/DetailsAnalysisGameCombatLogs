using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Entities;

public class CombatUnit : CombatDataBase
{
    public const int GAMEID_MAX_LENGTH = 128;
    public const int USERNAME_MAX_LENGTH = 128;

    private CombatUnit() { }

    private CombatUnit(string gameId, string username, string? creatorGameId)
    {
        Id = Guid.NewGuid().ToString();
        GameId = gameId;
        Username = username;
        CreatorGameId = creatorGameId;
    }

    public string Id { get; private set; } = string.Empty;

    public string GameId { get; private set; } = string.Empty;

    public string Username { get; private set; } = string.Empty;

    public string? CreatorGameId { get; private set; }

    public Combat Combat { get; private set; }

    public static CombatUnit Create(string gameId, string username, string? creatorGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));

        return new CombatUnit(gameId, username, creatorGameId);
    }
}
