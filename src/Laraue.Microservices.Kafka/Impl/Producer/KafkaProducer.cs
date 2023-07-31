using Confluent.Kafka;
using Laraue.Microservices.Kafka.Abstractions.Producer;

namespace Laraue.Microservices.Kafka.Impl.Producer;

public sealed class KafkaProducer<TMessage> : IKafkaProducer<TMessage> 
    where TMessage : class
{
    private readonly IProducer<string, TMessage> _producer;

    public KafkaProducer(
        IProducer<string, TMessage> producer,
        string topicName)
    {
        _producer = producer;
        Topic = topicName;
    }

    public string Topic { get; }

    public Task ProduceAsync(string key, TMessage message, Headers headers, CancellationToken ct = default)
    {
        return _producer.ProduceAsync(
            Topic,
            new Message<string, TMessage>
            {
                Key = key,
                Value = message,
                Timestamp = Timestamp.Default,
                Headers = headers,
            },
            ct);
    }

    public Task ProduceAsync(string key, TMessage message, CancellationToken ct = default)
    {
        return ProduceAsync(key, message, new Headers(), ct);
    }
}