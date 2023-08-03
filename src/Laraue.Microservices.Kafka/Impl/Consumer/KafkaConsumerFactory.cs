using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Laraue.Microservices.Kafka.Abstractions.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Microservices.Kafka.Impl.Consumer;

public sealed class KafkaConsumerFactory : IKafkaConsumerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RegisteredConsumers _registeredConsumers;

    public KafkaConsumerFactory(IServiceProvider serviceProvider, RegisteredConsumers registeredConsumers)
    {
        _serviceProvider = serviceProvider;
        _registeredConsumers = registeredConsumers;
    }

    public IKafkaConsumer<TMessage> GetKafkaConsumer<TMessage>() where TMessage : class
    {
        return _serviceProvider.GetRequiredService<IKafkaConsumer<TMessage>>();
    }

    public IKafkaConsumer<TMessage> GetKafkaConsumer<TMessage>(string consumerKey) where TMessage : class
    {
        if (!_registeredConsumers.TryGetValue(consumerKey, out var consumer))
        {
            throw new InvalidOperationException($"Consumer with the key {consumerKey} is not registered");
        }

        if (consumer is not IKafkaConsumer<TMessage> typedConsumer)
        {
            throw new InvalidOperationException(
                $"Producer with the key {consumerKey} is of type {consumer.GetType()}," +
                $" but {typeof(IKafkaProducer<TMessage>)} was requested");
        }

        return typedConsumer;
    }
}