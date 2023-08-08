using System.Text.Json;
using Laraue.Microservices.Apps.KafkaProducer;
using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Laraue.Microservices.Kafka.Impl.Consumer;

namespace Laraue.Microservices.Apps.Kafka;

public class Consumer : KafkaConsumerWorker<TestMessage>
{
    private readonly ILogger<Consumer> _logger;

    public Consumer(IKafkaConsumer<TestMessage> consumer, ILogger<Consumer> logger)
        : base(consumer)
    {
        _logger = logger;
    }

    protected override Task ProcessAsync(string key, TestMessage message)
    {
        _logger.LogInformation("{key}: {message} has been processed", key, JsonSerializer.Serialize(message));

        return Task.CompletedTask;
    }
}