using System.Text.Json;
using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Impl.Serializers;

public sealed class JsonSerializer<TMessage> : ISerializer<TMessage>
{
    private readonly JsonSerializerOptions? _options;

    public JsonSerializer()
    {
    }
    
    public JsonSerializer(JsonSerializerOptions options)
    {
        _options = options;
    }
    
    public byte[] Serialize(TMessage data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data, _options);
    }
}