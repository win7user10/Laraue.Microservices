using Laraue.Microservices.Kafka.Abstractions.Consumer;
using Laraue.Microservices.Kafka.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Microservices.Tests.Kafka.Consumer;

public class DependencyInjectionTests
{
    [Fact]
    public void Resolving_Directly()
    {
        var sc = new ServiceCollection()
            .AddKafkaConsumer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message")
                    .WithGroupId("1"));

        var sp = sc.BuildServiceProvider();
        var producer = sp.GetRequiredService<IKafkaConsumer<TestKafkaMessage>>();
        
        Assert.Equal("test-message", producer.Topic);
    }
    
    [Fact]
    public void Resolving_ViaFactory()
    {
        var sc = new ServiceCollection()
            .AddKafkaConsumer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message")
                    .WithGroupId("1"));

        var sp = sc.BuildServiceProvider();
        var producerFactory = sp.GetRequiredService<IKafkaConsumerFactory>();
        var producer = producerFactory.GetKafkaConsumer<TestKafkaMessage>();
        
        Assert.Equal("test-message", producer.Topic);
    }
    
    [Fact]
    public void DoubleAdding_IsNotAllowed()
    {
        Assert.Throws<InvalidOperationException>(
            () => new ServiceCollection()
                .AddKafkaConsumer<TestKafkaMessage>(b =>
                    b.WithTopicName("test-message")
                        .WithGroupId("1"))
                .AddKafkaConsumer<TestKafkaMessage>(b =>
                    b.WithTopicName("test-message")
                        .WithGroupId("1")));
    }
    
    [Fact]
    public void DoubleAdding_WithDifferentKeys_IsAllowed()
    {
        var sc = new ServiceCollection()
            .AddKafkaConsumer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message1")
                    .WithGroupId("1"), "producer1")
            .AddKafkaConsumer<TestKafkaMessage>(b =>
                b.WithTopicName("test-message2")
                    .WithGroupId("1"), "producer2");

        var sp = sc.BuildServiceProvider();
        var producerFactory = sp.GetRequiredService<IKafkaConsumerFactory>();
        
        var producer1 = producerFactory.GetKafkaConsumer<TestKafkaMessage>("producer1");
        var producer2 = producerFactory.GetKafkaConsumer<TestKafkaMessage>("producer2");
        
        Assert.Equal("test-message1", producer1.Topic);
        Assert.Equal("test-message2", producer2.Topic);
    }
}