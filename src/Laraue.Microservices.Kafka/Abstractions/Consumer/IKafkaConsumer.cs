using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Abstractions.Consumer;

public interface IKafkaConsumer<TMessage> : IKafkaConsumer
    where TMessage : class
{
    Task ConsumeAsync(
        Func<string, TMessage, Task> processAction,
        CancellationToken ct = default);
    
    Task ConsumeAsync(
        Func<ConsumeResult<string, TMessage>, Task> processAction,
        CancellationToken ct = default);
}

public interface IKafkaConsumer
{
    string Topic { get; }
}