using Laraue.Microservices.Metrics.Abstractions;

namespace Laraue.Microservices.Metrics.Impl;

public sealed class MetricsFactory : IMetricsFactory
{
    public ICounterMetric GetCounter(string key, string description)
    {
        return new CounterMetric(key, description);
    }
}