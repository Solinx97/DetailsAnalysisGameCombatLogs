using Chat.Application.Consts;
using CombatAnalysis.ChatAPI.Core;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatAPI.Kafka;

public abstract class KafkaConsumerBase : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly string _topic;
    private readonly ILogger _logger;
    private IConsumer<string, string>? _consumer;

    protected KafkaConsumerBase(IOptions<KafkaSettings> kafkaSettings, string topic, ILogger logger)
    {
        _config = kafkaSettings.Value.Consumer;
        _config.BootstrapServers = kafkaSettings.Value.BootstrapServers;
        _topic = topic;
        _logger = logger;
    }

    public override void Dispose()
    {
        _consumer?.Dispose();

        base.Dispose();
    }

    protected abstract Task ConsumeMessageAsync(ConsumeResult<string, string> result, CancellationToken stoppingToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig(_config);
        _consumer = new ConsumerBuilder<string, string>(consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            //.SetValueDeserializer(new JsonDocumentDeserializer())
            .SetErrorHandler((_, e) => _logger.LogError($"Kafka Consumer Error: {e.Reason}"))
            .SetStatisticsHandler((_, json) => _logger.LogDebug($"Kafka Statistics: {json}"))
            .Build();

        _consumer.Subscribe(_topic);
        _logger.LogInformation($"Kafka consumer subscribed to topic '{_topic}' with GroupId '{_config.GroupId}'.");

        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(stoppingToken);

                    if (consumeResult?.Message != null)
                    {
                        _logger.LogDebug($"Received message from '{_topic}' - Partition: {consumeResult.Partition}, Offset: {consumeResult.Offset}");

                        await ConsumeMessageAsync(consumeResult, stoppingToken);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Consume error on topic '{_topic}': {ex.Error.Reason}");
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An unexpected error occurred while consuming from '{_topic}': {ex.Message}");
                }
            }

            _consumer.Close();
            _logger.LogInformation($"Kafka consumer for topic '{_topic}' stopped.");
        }, stoppingToken);
    }
}
