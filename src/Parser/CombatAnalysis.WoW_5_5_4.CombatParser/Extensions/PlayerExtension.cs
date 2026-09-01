using CombatAnalysis.WoW_5_5_4.CombatParser.Consts;
using CombatAnalysis.WoW_5_5_4.CombatParser.Entities;
using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Extensions;

internal static class PlayerExtension
{
    public static async Task<Player?> LoadAsync(this Player player, IHttpClientHelper httpHelper, ILogger logger)
    {
        try
        {
            httpHelper.BaseAddress = API.CombatParserApi;

            var response = await httpHelper.GetAsync($"Player/getByGamePlayerId/{player.GameId}");
            response.EnsureSuccessStatusCode();

            var loadedPlayer = await response.Content.ReadFromJsonAsync<Player>();
            ArgumentNullException.ThrowIfNull(loadedPlayer, nameof(loadedPlayer));

            return loadedPlayer;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, ex.Message);

            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }

    public static async Task<Player?> CreateAsync(this Player player, IHttpClientHelper httpHelper, ILogger logger)
    {
        try
        {
            httpHelper.BaseAddress = API.CombatParserApi;

            var response = await httpHelper.PostAsync($"Player", JsonContent.Create(player));
            response.EnsureSuccessStatusCode();

            var createdPlayer = await response.Content.ReadFromJsonAsync<Player>();
            ArgumentNullException.ThrowIfNull(createdPlayer, nameof(createdPlayer));

            return createdPlayer;
        }
        catch (ArgumentNullException ex)
        {
            logger.LogError(ex, ex.Message);

            return null;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return null;
        }
    }
}
