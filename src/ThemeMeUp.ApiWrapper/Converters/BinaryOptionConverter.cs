using System;
using Newtonsoft.Json;

namespace ThemeMeUp.ApiWrapper.Converters
{
    public class BinaryOptionConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            var inputString = (string)reader.Value;

            return Activator.CreateInstance(typeof(T), inputString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
