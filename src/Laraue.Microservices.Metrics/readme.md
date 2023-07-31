Service responsibility
1. Contains adapters for fast metrics integration in IO bounded services (HTTP/GRPC) 
```csharp
class MetricsHttpHandler : HttpHandler
{
    override Task<HttpResponse> SendAsync();
}
```
```csharp
class GRPCMiddleware : Middleware
{
}
```

2. Contains adapters for fast metrics integration in DB calls
```csharp
DbSet
    .Where(x => x.Name == name)
    .WithMetricName("UsersQuery")
    .ToListAsync();
```

So, the base class for metric can be here
```csharp
class IOCallMetric
{
    string MetricType => "DB / Kafka producing / Kafka Consuming / HttpCall"
    string Data => "query name / topic name / endpoint name"
}
```