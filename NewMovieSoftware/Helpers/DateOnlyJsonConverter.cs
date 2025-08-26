using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MovieOrganiser2000.Helpers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
            => writer.WriteValue(value.ToString(Format));

        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && reader.Value is string s && DateOnly.TryParse(s, out var date))
                return date;
            // fallback: prøv via DateTime
            if (reader.TokenType == JsonToken.Date && reader.Value is DateTime dt)
                return DateOnly.FromDateTime(dt);
            return default;
        }
    }
}
