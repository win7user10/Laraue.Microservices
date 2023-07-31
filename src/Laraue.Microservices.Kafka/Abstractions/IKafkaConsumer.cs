namespace Laraue.Microservices.Kafka.Abstractions;

public interface IKafkaConsumer
{
    Task ProduceAsync();
}