using CombatAnalysis.Identity.Interfaces;

namespace HealthAPI.Services;

public class AuthCodeCleanupService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);
    private Timer _timer;

    public AuthCodeCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoCleanup, null, TimeSpan.Zero, _interval);

        return Task.CompletedTask;
    }

    private void DoCleanup(object state)
    {
        using var scope = _serviceProvider.CreateScope();
        var authCodeService = scope.ServiceProvider.GetRequiredService<IAuthCodeService>();

        authCodeService.RemoveExpiredCodes();
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
