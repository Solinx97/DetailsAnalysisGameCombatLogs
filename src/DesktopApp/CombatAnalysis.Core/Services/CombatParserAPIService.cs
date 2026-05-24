using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;

namespace CombatAnalysis.Core.Services;

internal class CombatParserAPIService : ICombatParserAPIService
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger _logger;
    private readonly IMemoryCache _memoryCache;

    public CombatParserAPIService(IHttpClientHelper httpClient, ILogger logger, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;

        _httpClient.BaseAddress = API.CombatParserApi;
    }

    public async Task SaveAsync(List<CombatModel> combats, CombatLogModel combatLog, Action<string, string> uplodedCallback, Func<CancellationToken> requestCancelationToken)
    {
        var readyCombatsNumber = 0;
        var cancellationToken = requestCancelationToken();

        var combatTasks = combats.Select(async combat =>
        {
            try
            {
                combat.CombatLogId = combatLog.Id;

                using var response = await _httpClient.PostAsync("Combat", JsonContent.Create(combat), cancellationToken);
                response.EnsureSuccessStatusCode();

                uplodedCallback(combat.DungeonName, combat.Boss.Name);

                readyCombatsNumber++;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Request was canceled by client: {Message}", ex.Message);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
            }
        });

        await Task.WhenAll(combatTasks);
    }

    public async Task DeleteCombatLogByUserAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.DeletAsync($"CombatLog/{id}", cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
        }
    }

    public async Task<IEnumerable<CombatLogModel>> LoadCombatLogsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync("CombatLog", cancellationToken);
            response.EnsureSuccessStatusCode();

            var combatLogs = await response.Content.ReadFromJsonAsync<IEnumerable<CombatLogModel>>();
            ArgumentNullException.ThrowIfNull(combatLogs, nameof(combatLogs));

            return combatLogs;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return [];
        }
    }

    public async Task<IEnumerable<CombatModel>> LoadCombatsAsync(int combatLogId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Combat/getByCombatLogId/{combatLogId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            var combats = await response.Content.ReadFromJsonAsync<IEnumerable<CombatModel>>(cancellationToken);
            ArgumentNullException.ThrowIfNull(combats, nameof(combats));

            return combats;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return [];
        }
    }

    public async Task<IEnumerable<CombatPlayerModel>> LoadCombatPlayersAsync(int combatId, CancellationToken cancellationToke)
    {
        try
        {
            var response = await _httpClient.GetAsync($"CombatPlayer/getByCombatId/{combatId}", cancellationToke);
            response.EnsureSuccessStatusCode();

            var combatPlayers = await response.Content.ReadFromJsonAsync<IEnumerable<CombatPlayerModel>>(cancellationToke);
            ArgumentNullException.ThrowIfNull(combatPlayers, nameof(combatPlayers));

            return combatPlayers;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Some arguments is null: {Message}", ex.Message);

            return [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return [];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return [];
        }
    }

    public async Task<int> LoadCountAsync(string address, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync(address, cancellationToken);
            response.EnsureSuccessStatusCode();

            var details = await response.Content.ReadFromJsonAsync<int>(cancellationToken);

            return details;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return 0;
        }
    }

    public async Task<CombatLogModel> SaveCombatLogAsync(List<CombatModel> combats, LogType logType, CancellationToken cancellationToken)
    {
        try
        {
            var user = _memoryCache.Get<AppUserModel>(nameof(MemoryCacheValue.User));
            ArgumentNullException.ThrowIfNull(user, nameof(user));

            var dungeonNames = combats
                 .GroupBy(group => group.DungeonName)
                 .Select(select => select.Key)
                 .Where(name => !string.IsNullOrEmpty(name))
                 .ToList();

            var name = CreateCombatLogName(dungeonNames);

            var combatLog = new CombatLogModel
            {
                Name = name,
                Date = DateTimeOffset.UtcNow,
                LogType = (int)logType,
                AppUserId = user.Id,
            };

            var response = await _httpClient.PostAsync("CombatLog", JsonContent.Create(combatLog), cancellationToken);
            response.EnsureSuccessStatusCode();

            var createdCombatLog = await response.Content.ReadFromJsonAsync<CombatLogModel>(cancellationToken);
            ArgumentNullException.ThrowIfNull(createdCombatLog, nameof(createdCombatLog));

            return createdCombatLog;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Some arguments is null: {Message}", ex.Message);

            return new CombatLogModel();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new CombatLogModel();
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Request was canceled by client: {Message}", ex.Message);

            return new CombatLogModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

            return new CombatLogModel();
        }
    }

    public async Task GetBossAsync(List<CombatModel> combats, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var combat in combats)
            { 
                var boss = await LoadBossAsync(combat.Boss.GameId, combat.Boss.Difficult, combat.Boss.Size, cancellationToken);
                combat.Boss = boss ?? new();
            }

            GetBossHealthPercentage(combats);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
        }
    }

    private async Task<BossModel?> LoadBossAsync(int gameBossId, int difficult, int groupSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Boss?gameBossId={gameBossId}&difficult={difficult}&groupSize={groupSize}", cancellationToken);
        response.EnsureSuccessStatusCode();

        var boss = await response.Content.ReadFromJsonAsync<BossModel>(cancellationToken: cancellationToken);

        return boss;
    }

    private static string CreateCombatLogName(List<string> dungeonNames)
    {
        var combatLogDungeonName = new StringBuilder();
        foreach (var item in dungeonNames)
        {
            var dungeonName = item.Trim('"');
            combatLogDungeonName.Append($"{dungeonName}/");
        }

        combatLogDungeonName.Remove(combatLogDungeonName.Length - 1, 1);

        return combatLogDungeonName.ToString();
    }

    private static void GetBossHealthPercentage(List<CombatModel> combats)
    {
        foreach (var item in combats)
        {
            if (item.IsWin)
            {
                continue;
            }

            var damageToBoss = item.CombatPlayers.Sum(x => x.DamageDoneToBoss);
            var leftHealth = item.Boss.Health - damageToBoss;
            var precentage = (double)leftHealth / (double)item.Boss.Health;

            item.BossHealthPercentage = precentage < 0 ? 0 : Math.Round(precentage * 100, 2);
        }
    }
}
