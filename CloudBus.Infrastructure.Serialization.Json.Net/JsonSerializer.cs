using System;
using CloudBus.Core;
using Newtonsoft.Json;

namespace CloudBus.Infrastructure.Serialization.Json.Net
{
    public class JsonSerializer : IMessageSerializer
    {
        private readonly JsonSerializerSettings settings;

        public JsonSerializer()
        {
            settings = new JsonSerializerSettings();
        }

        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string Serialize<TMessage>(TMessage message)
        {
            return JsonConvert.SerializeObject(message, settings);
        }

        public object Deserialize(Type type, string data)
        {
            return JsonConvert.DeserializeObject(data, type);
        }
    }
}
