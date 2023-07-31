namespace Laraue.Microservices.Kafka.Abstractions.Producer;

public interface IKafkaProducerFactory
{
    IKafkaProducer<TMessage> GetKafkaProducer<TMessage>() where TMessage : class;
    IKafkaProducer<TMessage> GetKafkaProducer<TMessage>(string producerKey)
        where TMessage : class;
}