using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character;

namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;

public class AccountDetailsDto
{
    public CharacterSummaryDto Summary { get; set; } = new();

    public double MythicKeystoneRating { get; set; } = new();

    public int AchievementsReceived { get; set; }

    public int AchievementsCount { get; set; }

    public int MountsReceived { get; set; }

    public int MountsCount{ get; set; }

    public int PetsReceived { get; set; }

    public int PetsCount { get; set; }

    public int ToysReceived { get; set; }

    public int ToysCount { get; set; }

    public int DecorsReceived { get; set; }

    public int DecorsCount { get; set; }

    public int SetTransmogsReceived { get; set; }

    public int SetTransmogsCount { get; set; }

    public int TransmogsReceived { get; set; }

    public int TransmogsCount { get; set; }

    public int CharactersCount { get; set; }

    public int MaxLevelCharactersCount { get; set; }
}
