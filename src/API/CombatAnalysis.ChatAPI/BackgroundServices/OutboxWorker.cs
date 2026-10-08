using Chat.Domain.Repositories;
using CombatAnalysis.ChatAPI.Interfaces;

namespace CombatAnalysis.ChatAPI.BackgroundServices;

public class OutboxWorker(IServiceScopeFactory scopeFactory, IKafkaProducerService<string, string> kafkaProducer, ILogger<OutboxWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var messages = await repository.GetPendingAsync(100, cancellationToken);

                foreach (var message in messages)
                {
                    try
                    {
                        await kafkaProducer.ProduceAsync(message.Topic, message.Key, message.Payload, cancellationToken);

                        repository.MarkAsProcessed(message);

                        await unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to publish outbox message {MessageId}", message.Id);
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Outbox worker failed");
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
    }
}
