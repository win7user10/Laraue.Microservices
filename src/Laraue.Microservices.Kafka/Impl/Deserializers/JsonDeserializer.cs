using System.Text.Json;
using Confluent.Kafka;

namespace Laraue.Microservices.Kafka.Impl.Deserializers;

public sealed class JsonDeserializer<TMessage> : IDeserializer<TMessage>
{
#pragma warning disable CS8766
    public TMessage? Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
#pragma warning restore CS8766
    {
        return isNull
            ? default
            : JsonSerializer.Deserialize<TMessage>(data);
    }
}