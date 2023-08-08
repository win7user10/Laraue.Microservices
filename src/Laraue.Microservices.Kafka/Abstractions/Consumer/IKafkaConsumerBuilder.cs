using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Abstractions.Consumer;

public interface IKafkaConsumerBuilder<TMessage> where TMessage : class
{
    IKafkaConsumerBuilder<TMessage> WithTopicName(string topicName);
    
    IKafkaConsumerBuilder<TMessage> WithGroupId(string groupId);
    
    IKafkaConsumerBuilder<TMessage> WithConfiguration(ConsumerConfig consumerConfig);

    IKafkaConsumerBuilder<TMessage> WithConfiguration(ConsumerOptions consumerOptions);
    
    IKafkaConsumerBuilder<TMessage> WithKeyDeserializer(IDeserializer<string> deserializer);
    
    IKafkaConsumerBuilder<TMessage> WithValueDeserializer(IDeserializer<TMessage> deserializer);

    IKafkaConsumerBuilder<TMessage> ConfigureConfluentConsumer(
        Action<ConsumerBuilder<string, TMessage>> configureConsumer);

    internal IKafkaConsumer<TMessage> Build();
}