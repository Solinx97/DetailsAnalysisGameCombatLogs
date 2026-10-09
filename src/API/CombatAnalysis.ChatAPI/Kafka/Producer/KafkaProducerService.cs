using Chat.Application.Consts;
using Chat.Application.DTOs;
using CombatAnalysis.ChatAPI.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text;

namespace CombatAnalysis.ChatAPI.Kafka.Producer;

internal class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;

    public KafkaProducerService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaProducerService> logger)
    {
        var producerConfig = kafkaSettings.Value.Producer ?? new ProducerConfig();
        producerConfig.BootstrapServers ??= kafkaSettings.Value.BootstrapServers;

        if (string.IsNullOrEmpty(producerConfig.BootstrapServers))
        {
            throw new ArgumentException("Kafka BootstrapServers configuration is missing or invalid.");
        }

        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        _logger = logger;
    }

    public async Task ProduceAsync(OutboxMessageDto outboxMessage, CancellationToken stoppingToken)
    {
        try
        {
            var message = new Message<string, string>
            {
                Key = outboxMessage.Key,
                Value = outboxMessage.Payload,
                Headers = new Headers
                {
                    {
                        "event-type",
                        Encoding.UTF8.GetBytes(outboxMessage.EventType)
                    }
                }
            };
            var deliveryResult = await _producer.ProduceAsync(outboxMessage.Topic, message, stoppingToken);

            _logger.LogInformation($"Message delivered to '{outboxMessage.Topic}' - Partition: {deliveryResult.Partition}, Offset: {deliveryResult.Offset}");
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError($"Failed to deliver message to '{outboxMessage.Topic}': {ex.Error.Reason}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while producing to '{outboxMessage.Topic}': {ex.Message}");
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}
