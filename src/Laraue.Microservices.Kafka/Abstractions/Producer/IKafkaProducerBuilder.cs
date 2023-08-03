using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Laraue.Microservices.Kafka.Abstractions.Producer;

public interface IKafkaProducerBuilder<TMessage> where TMessage : class
{
    IKafkaProducerBuilder<TMessage> WithTopicName(string topicName);
    
    IKafkaProducerBuilder<TMessage> WithConfiguration(ProducerConfig producerConfig);
    
    IKafkaProducerBuilder<TMessage> WithConfiguration(ProducerOptions producerOptions);
    
    IKafkaProducerBuilder<TMessage> WithKeySerializer(ISerializer<string> serializer);
    
    IKafkaProducerBuilder<TMessage> WithValueSerializer(ISerializer<TMessage> serializer);

    IKafkaProducerBuilder<TMessage> ConfigureConfluentProducer(
        Action<ProducerBuilder<string, TMessage>> configureProducer);

    internal IKafkaProducer<TMessage> Build();
}