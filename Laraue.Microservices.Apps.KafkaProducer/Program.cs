using Laraue.Microservices.Apps.KafkaProducer;
using Laraue.Microservices.Common.Configuration;
using Laraue.Microservices.Kafka.Abstractions.Producer;
using Laraue.Microservices.Kafka.Extensions;
using Laraue.Microservices.Metrics.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var producerOptions = builder.Configuration.GetSection("Kafka:Producers:TestEntityPublisher").GetOrThrow<ProducerOptions>();
services.AddKafkaProducer<TestMessage>(b => b.WithConfiguration(producerOptions));
services.AddHostedService<BackgroundProducer>();

var app = builder.Build();

app.UseMetrics();
app.MapGet("/", () => "Hello World!");

app.Run();