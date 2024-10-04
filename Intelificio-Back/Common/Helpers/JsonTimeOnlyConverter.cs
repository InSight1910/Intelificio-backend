using System.Text.Json;
using System.Text.Json.Serialization;
using Org.BouncyCastle.Asn1.X509;

namespace Backend.Common.Helpers;

public class JsonTimeOnlyConverter : JsonConverter<TimeOnly>
{
    private readonly string _format = "HH:mm";

    public JsonTimeOnlyConverter()
    {
    }

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.ParseExact(reader.GetString(), _format, null);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_format));
    }
}