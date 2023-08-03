using Laraue.Microservices.Metrics.Abstractions;
using Laraue.Microservices.Metrics.Impl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Laraue.Microservices.Metrics.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryAddMetrics(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<IMetricsFactory, MetricsFactory>();

        return serviceCollection;
    }
}