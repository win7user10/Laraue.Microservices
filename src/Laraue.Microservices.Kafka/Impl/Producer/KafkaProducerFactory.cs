using Laraue.Microservices.Kafka.Abstractions.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Microservices.Kafka.Impl.Producer;

public sealed class KafkaProducerFactory : IKafkaProducerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RegisteredProducers _registeredProducers;

    public KafkaProducerFactory(IServiceProvider serviceProvider, RegisteredProducers registeredProducers)
    {
        _serviceProvider = serviceProvider;
        _registeredProducers = registeredProducers;
    }
    
    public IKafkaProducer<TMessage> GetKafkaProducer<TMessage>() where TMessage : class
    {
        return _serviceProvider.GetRequiredService<IKafkaProducer<TMessage>>();
    }

    public IKafkaProducer<TMessage> GetKafkaProducer<TMessage>(string producerKey) where TMessage : class
    {
        if (!_registeredProducers.TryGetValue(producerKey, out var producer))
        {
            throw new InvalidOperationException($"Producer with the key {producerKey} is not registered");
        }

        if (producer is not IKafkaProducer<TMessage> typedProducer)
        {
            throw new InvalidOperationException(
                $"Producer with the key {producerKey} is of type {producer.GetType()}," +
                $" but {typeof(IKafkaProducer<TMessage>)} was requested");
        }

        return typedProducer;
    }
}