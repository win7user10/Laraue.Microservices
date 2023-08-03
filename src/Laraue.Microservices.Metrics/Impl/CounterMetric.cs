using Prometheus;

namespace Laraue.Microservices.Metrics.Impl;

public sealed class CounterMetric : ICounterMetric
{
    private readonly Counter _counter;

    public CounterMetric(string key, string description)
    {
        _counter = Prometheus
            .Metrics
            .CreateCounter(key, description);
    }
    
    public void Increment()
    {
        _counter.Inc();
    }
}