using System.Text.Json;
using System.Text.Json.Serialization;

namespace FLyTicketService.Shared
{
    public class EnumConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();
            if (Enum.TryParse<T>(value, out T result))
            {
                return result;
            }
            throw new JsonException($"Invalid value for {typeof(T).Name}: {value}");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

}
