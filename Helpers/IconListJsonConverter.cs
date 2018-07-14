using CodeStack.Community.Sw.MyToolbar.Preferences;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CodeStack.Community.Sw.MyToolbar.Helpers
{
    public class IconListJsonConverter : JsonConverter
    {
        private readonly Type[] m_KnownTypes = new Type[]
        {
            typeof(HighResIcons),
            typeof(BasicIcons),
            typeof(MasterIcons)
        };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IIconList);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObj = JObject.Load(reader);

            var type = m_KnownTypes.FirstOrDefault(t => Matches(t, jObj));

            if (type == null)
            {
                throw new InvalidCastException("Failed to extract the icon information. Make sure that correct format is used");
            }

            return jObj.ToObject(type);
        }

        private bool Matches(Type type, JObject jObj)
        {
            var jPrps = jObj.Children<JProperty>().Select(p => p.Name).ToList();
            var tPrps = type.GetProperties().Select(p => p.Name).ToList();

            jPrps.Sort();
            tPrps.Sort();

            return jPrps.SequenceEqual(tPrps);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
