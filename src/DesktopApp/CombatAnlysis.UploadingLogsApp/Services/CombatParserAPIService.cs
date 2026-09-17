using AutoMapper;
using CombatAnalysis.UploadingLogsApp.Consts;
using CombatAnalysis.UploadingLogsApp.Core;
using CombatAnalysis.UploadingLogsApp.Enums;
using CombatAnalysis.UploadingLogsApp.Extensions;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Models;
using CombatAnalysis.UploadingLogsApp.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Services;

internal class CombatParserAPIService : ICombatParserAPIService
{
    private const int PARALLEL_COUNT = 3;
    private const int DEFAULT_RAID_SIZE = 30;

    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<CombatParserAPIService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public CombatParserAPIService(IHttpClientHelper httpClient, ILogger<CombatParserAPIService> logger, IMemoryCache memoryCache, IMapper mapper)
    {
        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;
        _mapper = mapper;

        _httpClient.BaseAddress = API.CombatParserApi;
    }

    public async Task SaveAsync(List<CombatModel> combats, int combatLogId, Action<string, string> uplodedCallback, Func<CancellationToken> requestCancellationToken)
    {
        var cancellationToken = requestCancellationToken();

        using var semaphore = new SemaphoreSlim(PARALLEL_COUNT);
        var combatTasks = combats.Select(async combat =>
        {
            await semaphore.WaitAsync(cancellationToken);

            try
            {
                var createCombat = _mapper.Map<CreateCombatModel>(combat);

                createCombat.GameVersion = (int)CurrentCombatParserVersion.Version;
                createCombat.CombatLogId = combatLogId;

                using var content = JsonContent.Create(createCombat);
                using var response = await _httpClient.PostAsync("Combat", content, cancellationToken, true);
                response.EnsureSuccessStatusCode();

                uplodedCallback(combat.DungeonName, combat.Boss.Name);

                combat.ReleaseParsedData();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError(ex, "Authorization failed: {Message}", ex.Message);

                combat.ReleaseParsedData();

                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

                combat.ReleaseParsedData();

                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Request was canceled by client: {Message}", ex.Message);

                combat.ReleaseParsedData();

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);

                combat.ReleaseParsedData();
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(combatTasks);

        await AddCombatLogCreatedStatusAsync(combatLogId, cancellationToken);

        combats.Clear();

        // Reduce capacity, provided to collections but not release after cleaning collection yet
        combats.TrimExcess();

        // Call GC to collect and release LOH right now
        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, blocking: true, compacting: true);
    }

    public async Task<int> SaveCombatLogAsync(List<CombatModel> combats, LogType logType, CancellationToken cancellationToken)
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
                GameVersion = (int)CurrentCombatParserVersion.Version,
            };

            var response = await _httpClient.PostAsync("CombatLog", JsonContent.Create(combatLog), cancellationToken, true);
            response.EnsureSuccessStatusCode();

            var createdCombatLogId = await response.Content.ReadFromJsonAsync<int>(cancellationToken);

            return createdCombatLogId;

        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Some arguments is null: {Message}", ex.Message);

            throw;
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

            throw;
        }
    }

    public async Task GetBossAsync(List<CombatModel> combats, bool useDefault, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var combat in combats)
            {
                var boss = await LoadBossAsync(combat.Boss.GameId, combat.Boss.Difficult, useDefault ? DEFAULT_RAID_SIZE : combat.Boss.Size, cancellationToken);
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

    private async Task AddCombatLogCreatedStatusAsync(int combatLogId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.PostAsync($"CombatLog/addStatus/{combatLogId}?status={(int)CombatLogStatus.Created}", JsonContent.Create(new {}), cancellationToken, true);
            response.EnsureSuccessStatusCode();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Some arguments is null: {Message}", ex.Message);

            throw;
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

            throw;
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

            var leftHealth = item.Boss.Health - 0;
            var precentage = (double)leftHealth / (double)item.Boss.Health;

            //item.BossHealthPercentage = precentage < 0 ? 0 : Math.Round(precentage * 100, 2);
            item.BossHealthPercentage = 100;
        }
    }
}
