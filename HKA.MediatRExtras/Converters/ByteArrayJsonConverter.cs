using Newtonsoft.Json;

namespace HKA.MediatRExtras.Converters;

/// <summary>
/// This will remove byte arrays from serialization
/// </summary>
internal class ByteArrayJsonConverter : JsonConverter<byte[]>
{
    public override byte[]? ReadJson(JsonReader reader, Type objectType, byte[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, byte[]? value, JsonSerializer serializer)
    {
        if (value is not null)
        {
            writer.WriteValue($"**Byte[](Size={value.Length}**");
        }
        else
        {
            writer.WriteNull();
        }
    }
}
