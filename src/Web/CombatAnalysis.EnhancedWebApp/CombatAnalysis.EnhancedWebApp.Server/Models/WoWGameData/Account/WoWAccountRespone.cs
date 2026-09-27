using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;

public class WoWAccountRespone
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("wow_accounts")]
    public WoWAccountModel[] WowAccounts { get; set; }
}
