using Confluent.Kafka;
using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Laraue.Microservices.Metrics;
using Laraue.Microservices.Metrics.Abstractions;

namespace Laraue.Microservices.Kafka.Impl.Consumer;

public sealed class KafkaConsumer<TMessage> : IKafkaConsumer<TMessage> where TMessage : class
{
    private readonly IConsumer<string, TMessage> _consumer;
    private readonly ICounterMetric _counter;

    public KafkaConsumer(
        IConsumer<string, TMessage> consumer,
        string topicName,
        IMetricsFactory metricsFactory)
    {
        _consumer = consumer;
        Topic = topicName;
        
        _counter = metricsFactory.GetCounter(
            $"kafka_consumer_{Topic}",
            "Amount of the messages consumed by the current consumer");
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
            
            _counter.Increment();
        }
        
        _consumer.Close();
    }

    public string Topic { get; }
}