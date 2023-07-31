using Confluent.Kafka;
using Laraue.Microservices.Kafka.Abstractions.Producer;
using Laraue.Microservices.Kafka.Impl.Serializers;

namespace Laraue.Microservices.Kafka.Impl;

public sealed class KafkaProducerBuilder<TMessage> : IKafkaProducerBuilder<TMessage>
    where TMessage : class
{
    private string? _topicName;
    private ProducerConfig _producerConfig = new ();
    private Action<ProducerBuilder<string, TMessage>> _configureConfluentProducer;

    public KafkaProducerBuilder()
    {
        _configureConfluentProducer = builder =>
            builder.SetKeySerializer(new JsonSerializer<string>())
                .SetValueSerializer(new JsonSerializer<TMessage>());
    }
    
    public IKafkaProducerBuilder<TMessage> WithTopicName(string topicName)
    {
        _topicName = topicName;

        return this;
    }

    public IKafkaProducerBuilder<TMessage> WithConfiguration(ProducerConfig producerConfig)
    {
        _producerConfig = producerConfig;

        return this;
    }

    public IKafkaProducerBuilder<TMessage> WithKeySerializer(ISerializer<string> serializer)
    {
        _configureConfluentProducer += builder => builder.SetKeySerializer(serializer);
        
        return this;
    }

    public IKafkaProducerBuilder<TMessage> WithValueSerializer(ISerializer<TMessage> serializer)
    {
        _configureConfluentProducer += builder => builder.SetValueSerializer(serializer);

        return this;
    }

    public IKafkaProducerBuilder<TMessage> ConfigureConfluentProducer(
        Action<ProducerBuilder<string, TMessage>> configureProducer)
    {
        _configureConfluentProducer = configureProducer;

        return this;
    }

    public IKafkaProducer<TMessage> Build()
    {
        if (_topicName is null)
        {
            throw new InvalidOperationException("Topic is not set");
        }

        var producerBuilder = new ProducerBuilder<string, TMessage>(_producerConfig);
        _configureConfluentProducer?.Invoke(producerBuilder);

        return new KafkaProducer<TMessage>(producerBuilder.Build(), _topicName);
    }
}