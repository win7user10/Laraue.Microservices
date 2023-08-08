using System.Text;
using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Extensions;

public static class HeadersExtensions
{
    public static string? TryGetStringValue(this Headers headers, string name)
    {
        var headerBytes = headers.FirstOrDefault(x => x.Key == name)?.GetValueBytes();

        return headerBytes is null
            ? null
            : Encoding.UTF8.GetString(headerBytes);
    }
    
    public static ReadOnlySpan<byte> TryGetBytesValue(this Headers headers, string name)
    {
        return headers.FirstOrDefault(x => x.Key == name)?.GetValueBytes();
    }
}