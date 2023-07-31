using Microsoft.Extensions.Hosting;

namespace Laraue.Microservices.ApiGateway.Extensions;

public static class ServiceCollectionExtensions
{
    public static IHostBuilder UseApiGateway(this IHostBuilder builder)
    {
        return builder;
    }
}