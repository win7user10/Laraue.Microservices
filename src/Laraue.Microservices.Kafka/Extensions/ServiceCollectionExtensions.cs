using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Laraue.Microservices.Kafka.Abstractions.Producer;
using Laraue.Microservices.Kafka.Impl.Consumer;
using Laraue.Microservices.Kafka.Impl.Producer;
using Laraue.Microservices.Metrics.Extensions;
using Laraue.Microservices.Metrics.Impl;
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
        var kafkaProducerBuilder = new KafkaProducerBuilder<TMessage>(new MetricsFactory());
        
        useProducerBuilder(kafkaProducerBuilder);

        var kafkaProducer = kafkaProducerBuilder.Build();

        sc.TryAddSingleton<IKafkaProducerFactory, KafkaProducerFactory>();

        if (sc
            .FirstOrDefault(x => x.ServiceType == typeof(RegisteredProducers))
            ?.ImplementationInstance is not RegisteredProducers registeredProducers)
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
    
    public static IServiceCollection AddKafkaConsumerWorker<TWorker, TMessage>(
        this IServiceCollection sc)
        where TMessage : class
        where TWorker : KafkaConsumerWorker<TMessage>
    {
        return sc.AddHostedService<TWorker>();
    }
    
    public static IServiceCollection AddKafkaConsumer<TMessage>(
        this IServiceCollection sc,
        Action<IKafkaConsumerBuilder<TMessage>> useConsumerBuilder)
        where TMessage : class
    {
        return sc.AddKafkaConsumer(useConsumerBuilder, typeof(TMessage).Name);
    }
    
    public static IServiceCollection AddKafkaConsumer<TMessage>(
        this IServiceCollection sc,
        Action<IKafkaConsumerBuilder<TMessage>> useConsumerBuilder,
        string producerKey)
        where TMessage : class
    {
        var kafkaConsumerBuilder = new KafkaConsumerBuilder<TMessage>(new MetricsFactory());
        
        useConsumerBuilder(kafkaConsumerBuilder);

        var kafkaConsumer = kafkaConsumerBuilder.Build();

        sc.TryAddSingleton<IKafkaConsumerFactory, KafkaConsumerFactory>();

        if (sc
                .FirstOrDefault(x => x.ServiceType == typeof(RegisteredConsumers))
                ?.ImplementationInstance is not RegisteredConsumers registeredConsumers)
        {
            registeredConsumers = new RegisteredConsumers();
            sc.AddSingleton(registeredConsumers);
        }

        if (!registeredConsumers.TryAdd(producerKey, kafkaConsumer))
        {
            throw new InvalidOperationException(
                $"Consumer with the key {producerKey} has been already registered");
        }
        
        return sc.AddSingleton(kafkaConsumer);
    }
}