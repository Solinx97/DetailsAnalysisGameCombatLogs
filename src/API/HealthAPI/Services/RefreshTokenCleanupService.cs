using CombatAnalysis.Identity.Interfaces;

namespace HealthAPI.Services;

public class RefreshTokenCleanupService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);
    private Timer? _timer;

    public RefreshTokenCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async (state) => await DoCleanupAsync(state), null, TimeSpan.Zero, _interval);

        return Task.CompletedTask;
    }

    private async Task DoCleanupAsync(object? state)
    {
        using var scope = _serviceProvider.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        await tokenService.RemoveExpiredTokensAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
