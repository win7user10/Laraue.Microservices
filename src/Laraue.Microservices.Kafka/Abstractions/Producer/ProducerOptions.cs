using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Abstractions.Producer;

public sealed class ProducerOptions
{
    public required string Topic { get; init; }
    
    public ProducerConfig? ProducerConfig { get; init; }
}