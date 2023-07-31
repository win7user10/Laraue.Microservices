using Microsoft.Extensions.DependencyInjection;

namespace Laraue.Microservices.Http.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpClient<T>(this IServiceCollection sc)
    {
        return sc;
    }
}