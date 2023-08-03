namespace Laraue.Microservices.Metrics.Abstractions;

public interface IMetricsFactory
{
    ICounterMetric GetCounter(string key, string description);
}