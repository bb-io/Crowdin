using Apps.Crowdin.Models.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Crowdin.Converters;

public class FieldsConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(IEnumerable<FieldEntity>).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);

        if (token.Type == JTokenType.Object)
        {
            var dict = token.ToObject<Dictionary<string, object>>(serializer) ?? [];
            return dict.Select(x => new FieldEntity(x.Key, x.Value)).ToList();
        }

        return new List<FieldEntity>();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is IEnumerable<FieldEntity> list)
        {
            var dict = list.ToDictionary(x => x.FieldKey, x => (object)x.FieldValue);
            serializer.Serialize(writer, dict);
        }
        else
            writer.WriteNull();
    }
}
