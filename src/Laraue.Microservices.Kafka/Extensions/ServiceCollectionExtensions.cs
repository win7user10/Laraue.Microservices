using Laraue.Microservices.Kafka.Abstractions.Producer;
using Laraue.Microservices.Kafka.Impl;
using Laraue.Microservices.Kafka.Impl.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Laraue.Microservices.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaProducer<TMessage>(
        this IServiceCollection sc,
        Action<IKafkaProducerBuilder<TMessage>> useProducerBuilder)
        where TMessage : class
    {
        return sc.AddKafkaProducer(useProducerBuilder, typeof(TMessage).Name);
    }
    
    public static IServiceCollection AddKafkaProducer<TMessage>(
        this IServiceCollection sc,
        Action<IKafkaProducerBuilder<TMessage>> useProducerBuilder,
        string producerKey)
        where TMessage : class
    {
        var kafkaProducerBuilder = new KafkaProducerBuilder<TMessage>();
        
        useProducerBuilder(kafkaProducerBuilder);

        var kafkaProducer = kafkaProducerBuilder.Build();

        sc.TryAddSingleton<IKafkaProducerFactory, KafkaProducerFactory>();

        var registeredProducers = sc
            .FirstOrDefault(x => x.ServiceType == typeof(RegisteredProducers))
            ?.ImplementationInstance as RegisteredProducers;

        if (registeredProducers is null)
        {
            registeredProducers = new RegisteredProducers();
            sc.AddSingleton(registeredProducers);
        }

        if (!registeredProducers.TryAdd(producerKey, kafkaProducer))
        {
            throw new InvalidOperationException(
                $"Producer with the key {producerKey} has been already registered");
        }
        
        return sc.AddSingleton(kafkaProducer);
    }
}