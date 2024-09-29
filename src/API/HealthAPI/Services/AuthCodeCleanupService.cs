using CombatAnalysis.Identity.Interfaces;

namespace HealthAPI.Services;

public class AuthCodeCleanupService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public AuthCodeCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoCleanup, null, TimeSpan.Zero, TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }

    private void DoCleanup(object state)
    {
        using var scope = _serviceProvider.CreateScope();
        var authCodeService = scope.ServiceProvider.GetRequiredService<IAuthCodeService>();

        authCodeService.RemoveExpiredCodesAsync().Wait();
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
