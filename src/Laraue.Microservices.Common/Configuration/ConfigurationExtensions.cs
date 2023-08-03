using Microsoft.Extensions.Configuration;

namespace Laraue.Microservices.Common.Configuration;

public static class ConfigurationExtensions
{
    public static T GetOrThrow<T>(this IConfiguration configuration)
    {
        return configuration.Get<T>() ?? throw new InvalidOperationException("Configuration is empty for the section.");
    }
}