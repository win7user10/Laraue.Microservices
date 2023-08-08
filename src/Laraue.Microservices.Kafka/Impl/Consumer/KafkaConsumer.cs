using System.Diagnostics;
using Confluent.Kafka;
using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Laraue.Microservices.Kafka.Extensions;
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
            $"kafka_consumer_{Topic.Replace('-', '_')}",
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

            using var activity = StartActivity(consumeResult);

            await processAction(consumeResult);
            
            _counter.Increment();
        }
        
        _consumer.Close();
    }

    private Activity StartActivity(ConsumeResult<string, TMessage> consumeResult)
    {
        var parentActivityTraceId = consumeResult.Message.Headers.TryGetStringValue(Constants.ActivityTraceIdHeader);
        var parentActivitySpanId = consumeResult.Message.Headers.TryGetStringValue(Constants.ActivitySpanIdHeader);

        var parentTraceId = parentActivityTraceId is not null
            ? ActivityTraceId.CreateFromString(parentActivityTraceId)
            : ActivityTraceId.CreateRandom();
        
        var parentSpanId = parentActivitySpanId is not null
            ? ActivitySpanId.CreateFromString(parentActivitySpanId)
            : ActivitySpanId.CreateRandom();

        var activity = new Activity($"Kafka.Consumer.{Topic}");
        if (parentActivityTraceId is not null)
        {
            activity.SetParentId(parentTraceId, parentSpanId, ActivityTraceFlags.Recorded);
        }

        activity.Start();

        return activity;
    }

    public string Topic { get; }
}