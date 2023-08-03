using Laraue.Microservices.Kafka.Abstractions.Producer;

namespace Laraue.Microservices.Apps.KafkaProducer;

public class BackgroundProducer : BackgroundService
{
    private readonly IKafkaProducer<TestMessage> _producer;

    public BackgroundProducer(IKafkaProducer<TestMessage> producer)
    {
        _producer = producer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var id = Guid.NewGuid();
            
            await _producer.ProduceAsync(
                id.ToString(),
                new TestMessage(id, $"Alex_{id}"),
                stoppingToken);

            await Task.Delay(100, stoppingToken);
        }
    }
}