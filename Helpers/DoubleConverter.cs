using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace TileGame.Helpers
{
    public class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();
                if (double.TryParse(value, out double result))
                {
                    return result;
                }
                if (value.Equals("Infinity", StringComparison.OrdinalIgnoreCase))
                    return double.PositiveInfinity;
                if (value.Equals("-Infinity", StringComparison.OrdinalIgnoreCase))
                    return double.NegativeInfinity;
                if (value.Equals("NaN", StringComparison.OrdinalIgnoreCase))
                    return double.NaN;
            }
            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (double.IsInfinity(value))
            {
                writer.WriteStringValue(value == double.PositiveInfinity ? "Infinity" : "-Infinity");
            }
            else if (double.IsNaN(value))
            {
                writer.WriteStringValue("NaN");
            }
            else
            {
                writer.WriteNumberValue(value);
            }
        }
    }

}
