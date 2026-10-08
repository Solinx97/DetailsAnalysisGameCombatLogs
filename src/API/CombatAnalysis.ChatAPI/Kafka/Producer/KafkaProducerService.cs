using Chat.Application.Consts;
using CombatAnalysis.ChatAPI.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.ChatAPI.Kafka.Producer;

internal class KafkaProducerService<TKey, TValue> : IKafkaProducerService<TKey, TValue>
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ILogger<KafkaProducerService<TKey, TValue>> _logger;

    public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducerService<TKey, TValue>> logger)
    {
        var producerConfig = kafkaSettings.Value.Producer ?? new ProducerConfig();
        producerConfig.BootstrapServers ??= kafkaSettings.Value.BootstrapServers;

        if (string.IsNullOrEmpty(producerConfig.BootstrapServers))
        {
            throw new ArgumentException("Kafka BootstrapServers configuration is missing or invalid.");
        }

        _producer = new ProducerBuilder<TKey, TValue>(producerConfig).Build();
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, TKey key, TValue value, CancellationToken stoppingToken)
    {
        try
        {
            var message = new Message<TKey, TValue> { Key = key, Value = value };
            var deliveryResult = await _producer.ProduceAsync(topic, message, stoppingToken);

            _logger.LogInformation($"Message delivered to '{topic}' - Partition: {deliveryResult.Partition}, Offset: {deliveryResult.Offset}");
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            _logger.LogError($"Failed to deliver message to '{topic}': {ex.Error.Reason}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while producing to '{topic}': {ex.Message}");
        }
    }

    public async Task ProduceAsync(string topic, Message<TKey, TValue> message, CancellationToken stoppingToken)
    {
        try
        {
            var deliveryResult = await _producer.ProduceAsync(topic, message, stoppingToken);
            _logger.LogInformation($"Message delivered to '{topic}' - Partition: {deliveryResult.Partition}, Offset: {deliveryResult.Offset}");
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            _logger.LogError($"Failed to deliver message to '{topic}': {ex.Error.Reason}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while producing to '{topic}': {ex.Message}");
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}
