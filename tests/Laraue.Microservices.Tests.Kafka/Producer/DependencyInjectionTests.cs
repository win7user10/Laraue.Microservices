using Laraue.Microservices.Kafka.Abstractions.Producer;
using Laraue.Microservices.Kafka.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Microservices.Tests.Kafka.Producer;

public class DependencyInjectionTests
{
    [Fact]
    public void Resolving_Directly()
    {
        var sc = new ServiceCollection()
            .AddKafkaProducer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message"));

        var sp = sc.BuildServiceProvider();
        var producer = sp.GetRequiredService<IKafkaProducer<TestKafkaMessage>>();
        
        Assert.Equal("test-message", producer.Topic);
    }
    
    [Fact]
    public void Resolving_ViaFactory()
    {
        var sc = new ServiceCollection()
            .AddKafkaProducer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message"));

        var sp = sc.BuildServiceProvider();
        var producerFactory = sp.GetRequiredService<IKafkaProducerFactory>();
        var producer = producerFactory.GetKafkaProducer<TestKafkaMessage>();
        
        Assert.Equal("test-message", producer.Topic);
    }
    
    [Fact]
    public void DoubleAdding_IsNotAllowed()
    {
        Assert.Throws<InvalidOperationException>(
            () => new ServiceCollection()
                .AddKafkaProducer<TestKafkaMessage>(b =>
                    b.WithTopicName("test-message"))
                .AddKafkaProducer<TestKafkaMessage>(b =>
                    b.WithTopicName("test-message")));
    }
    
    [Fact]
    public void DoubleAdding_WithDifferentKeys_IsAllowed()
    {
        var sc = new ServiceCollection()
            .AddKafkaProducer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message1"), "producer1")
            .AddKafkaProducer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message2"), "producer2");

        var sp = sc.BuildServiceProvider();
        var producerFactory = sp.GetRequiredService<IKafkaProducerFactory>();
        
        var producer1 = producerFactory.GetKafkaProducer<TestKafkaMessage>("producer1");
        var producer2 = producerFactory.GetKafkaProducer<TestKafkaMessage>("producer2");
        
        Assert.Equal("test-message1", producer1.Topic);
        Assert.Equal("test-message2", producer2.Topic);
    }
}