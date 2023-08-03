using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace Laraue.Microservices.Metrics.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMetrics(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseRouting();
        
        return applicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapMetrics();
        });
    }
}