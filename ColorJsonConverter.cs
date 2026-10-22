using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab3_3_1;

public class ColorJsonConverter : JsonConverter<Color>
{
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                ColorTranslator.FromHtml(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, Color colorValue, JsonSerializerOptions options) =>
                writer.WriteStringValue(ColorTranslator.ToHtml(colorValue));
}
