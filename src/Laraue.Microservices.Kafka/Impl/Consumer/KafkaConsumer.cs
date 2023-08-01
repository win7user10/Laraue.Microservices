using Confluent.Kafka;
using Laraue.Microservices.Kafka.Abstractions.Consumer;

namespace Laraue.Microservices.Kafka.Impl.Consumer;

public sealed class KafkaConsumer<TMessage> : IKafkaConsumer<TMessage> where TMessage : class
{
    private readonly IConsumer<string, TMessage> _consumer;

    public KafkaConsumer(IConsumer<string, TMessage> consumer, string topicName)
    {
        _consumer = consumer;
        Topic = topicName;
    }
    
    public Task ConsumeAsync(Func<string, TMessage, Task> processAction, CancellationToken ct = default)
    {
        return ConsumeAsync(
            (res)
                => processAction(res.Message.Key, res.Message.Value),
            ct);
    }

    public async Task ConsumeAsync(Func<ConsumeResult<string, TMessage>, Task> processAction, CancellationToken ct = default)
    {
        _consumer.Subscribe(Topic);

        while (!ct.IsCancellationRequested)
        {
            var consumeResult = _consumer.Consume(ct);

            await processAction(consumeResult);
        }
        
        _consumer.Close();
    }

    public string Topic { get; }
}