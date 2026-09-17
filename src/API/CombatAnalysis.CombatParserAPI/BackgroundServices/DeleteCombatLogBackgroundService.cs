using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatParser.Application.Commands.DeleteCombatLog;
using MediatR;

namespace CombatAnalysis.CombatParserAPI.BackgroundServices;

public sealed class DeleteCombatLogBackgroundService(IDeleteCombatLogQueue queue, IServiceScopeFactory scopeFactory, ILogger<DeleteCombatLogBackgroundService> logger)
    : BackgroundService
{
    private readonly IDeleteCombatLogQueue _queue = queue;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<DeleteCombatLogBackgroundService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var job = await _queue.DequeueAsync(stoppingToken);

                using var scope = _scopeFactory.CreateScope();

                var mediator = scope.ServiceProvider
                    .GetRequiredService<IMediator>();

                await mediator.Send(new DeleteCombatLogCommand(job.Id), stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting CombatLog");
            }
        }
    }
}
