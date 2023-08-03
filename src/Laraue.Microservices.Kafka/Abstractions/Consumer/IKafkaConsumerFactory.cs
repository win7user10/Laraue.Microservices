namespace Laraue.Microservices.Kafka.Abstractions.Consumer;

public interface IKafkaConsumerFactory
{
    IKafkaConsumer<TMessage> GetKafkaConsumer<TMessage>() where TMessage : class;
    IKafkaConsumer<TMessage> GetKafkaConsumer<TMessage>(string consumerKey)
        where TMessage : class;
}