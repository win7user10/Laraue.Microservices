using System.ComponentModel.DataAnnotations;
using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Abstractions;

public sealed class KafkaProducerOptions : ProducerConfig
{
    [Required]
    public string TopicName { get; } = string.Empty;
    
    [Required]
    public string ProducerKey { get; } = string.Empty;
}