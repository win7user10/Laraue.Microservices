using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Abstractions.Producer;

public interface IKafkaProducer<in TMessage> : IBroker
    where TMessage : class
{
    Task ProduceAsync(
        string key,
        TMessage message,
        Headers headers,
        CancellationToken ct = default);
    
    Task ProduceAsync(
        string key,
        TMessage message,
        CancellationToken ct = default);
}