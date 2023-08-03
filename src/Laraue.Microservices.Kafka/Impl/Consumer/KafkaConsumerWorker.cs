using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Microsoft.Extensions.Hosting;

namespace Laraue.Microservices.Kafka.Impl.Consumer;

public abstract class KafkaConsumerWorker<TMessage> : BackgroundService where TMessage : class
{
    private readonly IKafkaConsumer<TMessage> _consumer;

    protected KafkaConsumerWorker(IKafkaConsumer<TMessage> consumer)
    {
        _consumer = consumer;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return _consumer.ConsumeAsync(ProcessAsync, stoppingToken);
    }

    protected abstract Task ProcessAsync(string key, TMessage message);
}