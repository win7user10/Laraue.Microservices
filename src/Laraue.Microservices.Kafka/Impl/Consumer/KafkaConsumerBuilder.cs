using Confluent.Kafka;
using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Laraue.Microservices.Kafka.Impl.Deserializers;
using Laraue.Microservices.Metrics.Abstractions;

namespace Laraue.Microservices.Kafka.Impl.Consumer;

public sealed class KafkaConsumerBuilder<TMessage> : IKafkaConsumerBuilder<TMessage>
    where TMessage : class
{
    private readonly IMetricsFactory _metricsFactory;
    private string? _topicName;
    private ConsumerConfig _consumerConfig = new ();
    private Action<ConsumerBuilder<string, TMessage>> _configureConfluentConsumer;
    
    public KafkaConsumerBuilder(IMetricsFactory metricsFactory)
    {
        _metricsFactory = metricsFactory;
        _configureConfluentConsumer = builder =>
            builder.SetKeyDeserializer(new JsonDeserializer<string>())
                .SetValueDeserializer(new JsonDeserializer<TMessage>());
    }
    
    public IKafkaConsumerBuilder<TMessage> WithTopicName(string topicName)
    {
        _topicName = topicName;

        return this;
    }

    public IKafkaConsumerBuilder<TMessage> WithGroupId(string groupId)
    {
        _consumerConfig.GroupId = groupId;

        return this;
    }

    public IKafkaConsumerBuilder<TMessage> WithConfiguration(ConsumerConfig consumerConfig)
    {
        _consumerConfig = consumerConfig;

        return this;
    }

    public IKafkaConsumerBuilder<TMessage> WithKeyDeserializer(IDeserializer<string> deserializer)
    {
        _configureConfluentConsumer += builder => builder.SetKeyDeserializer(deserializer);
        
        return this;
    }

    public IKafkaConsumerBuilder<TMessage> WithValueDeserializer(IDeserializer<TMessage> deserializer)
    {
        _configureConfluentConsumer += builder => builder.SetValueDeserializer(deserializer);
        
        return this;
    }

    public IKafkaConsumerBuilder<TMessage> ConfigureConfluentConsumer(Action<ConsumerBuilder<string, TMessage>> configureConsumer)
    {
        _configureConfluentConsumer += configureConsumer;

        return this;
    }

    public IKafkaConsumer<TMessage> Build()
    {
        if (_topicName is null)
        {
            throw new InvalidOperationException("Topic is not set");
        }

        var consumerBuilder = new ConsumerBuilder<string, TMessage>(_consumerConfig);
        _configureConfluentConsumer.Invoke(consumerBuilder);

        return new KafkaConsumer<TMessage>(consumerBuilder.Build(), _topicName, _metricsFactory);
    }
}