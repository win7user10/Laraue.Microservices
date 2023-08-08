using System.Diagnostics;
using Confluent.Kafka;
using Laraue.Microservices.Kafka.Abstractions.Producer;
using Laraue.Microservices.Metrics;
using Laraue.Microservices.Metrics.Abstractions;

namespace Laraue.Microservices.Kafka.Impl.Producer;

public sealed class KafkaProducer<TMessage> : IKafkaProducer<TMessage> 
    where TMessage : class
{
    private readonly IProducer<string, TMessage> _producer;
    private readonly ICounterMetric _counter;

    public KafkaProducer(
        IProducer<string, TMessage> producer,
        string topicName,
        IMetricsFactory metricsFactory)
    {
        Topic = topicName;
        
        _producer = producer;
        _counter = metricsFactory.GetCounter(
            $"kafka_producer_{Topic.Replace('-', '_')}",
            "Amount of the messages published by the current producer");
    }

    public string Topic { get; }

    public async Task ProduceAsync(string key, TMessage message, Headers headers, CancellationToken ct = default)
    {
        using var activity = AddActivityHeaders(headers);
        
        await _producer.ProduceAsync(
            Topic,
            new Message<string, TMessage>
            {
                Key = key,
                Value = message,
                Timestamp = Timestamp.Default,
                Headers = headers,
            },
            ct);
        
        _counter.Increment();
    }
    
    private Activity AddActivityHeaders(Headers headers)
    {
        var activity = Activity.Current ?? new Activity($"Kafka.Producer.{Topic}");

        var activityTraceId = new Span<byte>();
        activity.TraceId.CopyTo(activityTraceId);
        headers.Add(Constants.ActivityTraceIdHeader, activityTraceId.ToArray());
        
        var activitySpanId = new Span<byte>();
        activity.SpanId.CopyTo(activitySpanId);
        headers.Add(Constants.ActivitySpanIdHeader, activitySpanId.ToArray());

        return activity;
    }

    public Task ProduceAsync(string key, TMessage message, CancellationToken ct = default)
    {
        return ProduceAsync(key, message, new Headers(), ct);
    }
}