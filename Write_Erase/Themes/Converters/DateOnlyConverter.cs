using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Write_Erase.Themes.Converters
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string _format;

        public DateOnlyConverter()
        {
        }

        public DateOnlyConverter(string format)
        {
            _format = format;
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateOnly.ParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            var dateTimeOffset = new DateTimeOffset(value.Year, value.Month, value.Day, 0, 0, 0, TimeSpan.Zero);
            writer.WriteStringValue(dateTimeOffset.ToString(_format));
        }
    }
}