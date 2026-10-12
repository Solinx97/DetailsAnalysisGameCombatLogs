using Confluent.Kafka;
using Notification.Application.Security;

namespace Notification.Application.Consts;

public class KafkaSettings
{
    public string BootstrapServers { get; set; }

    public ProducerConfig Producer { get; set; }

    public ConsumerConfig Consumer { get; set; }

    public KafkaSecurity Security { get; set; }

    public int AccessTokenExpiresMins { get; set; }

}
