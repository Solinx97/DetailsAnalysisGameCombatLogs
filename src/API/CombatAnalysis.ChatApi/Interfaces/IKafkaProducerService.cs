using Confluent.Kafka;

namespace CombatAnalysis.ChatAPI.Interfaces;

public interface IKafkaProducerService<TKey, TValue> : IDisposable
{
    Task ProduceAsync(string topic, TKey key, TValue value, CancellationToken stoppingToken);

    Task ProduceAsync(string topic, Message<TKey, TValue> message, CancellationToken stoppingToken);
}
