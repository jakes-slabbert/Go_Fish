using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoFishHelpers
{
    public class DictionaryObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, object>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return ParseToken(JToken.Load(reader));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject.FromObject(value).WriteTo(writer);
        }

        private object ParseToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var dict = new Dictionary<string, object>();
                    foreach (var property in token.Children<JProperty>())
                    {
                        dict[property.Name] = ParseToken(property.Value);
                    }
                    return dict;

                case JTokenType.Array:
                    var list = new List<object>();
                    foreach (var item in token.Children())
                    {
                        list.Add(ParseToken(item));
                    }
                    return list;

                case JTokenType.Integer:
                    return token.ToObject<long>();

                case JTokenType.Float:
                    return token.ToObject<double>();

                case JTokenType.Boolean:
                    return token.ToObject<bool>();

                case JTokenType.String:
                    return token.ToString();

                case JTokenType.Null:
                    return null;

                default:
                    return token.ToString();
            }
        }
    }
}
