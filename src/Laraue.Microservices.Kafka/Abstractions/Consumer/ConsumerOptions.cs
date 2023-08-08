using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Abstractions.Consumer;

public sealed class ConsumerOptions
{
    public required string Topic { get; init; }
    
    public ConsumerConfig? ConsumerConfig { get; init; }
}