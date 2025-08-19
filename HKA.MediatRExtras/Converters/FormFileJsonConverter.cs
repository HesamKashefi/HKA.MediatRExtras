using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HKA.MediatRExtras.Converters;

/// <summary>
/// This will remove file from serialization
/// </summary>
internal class FormFileJsonConverter : JsonConverter<IFormFile>
{
    public override IFormFile? ReadJson(JsonReader reader, Type objectType, IFormFile? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, IFormFile? value, JsonSerializer serializer)
    {
        if (value is not null)
        {
            writer.WriteValue($"**FILE(Size={value.Length},ContentDisposition={value.ContentDisposition})**");
        }
        else
        {
            writer.WriteNull();
        }
    }
}
