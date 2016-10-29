using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace CloudBus.Core.Serialization
{
    public class JsonDataContractSerializer : IMessageSerializer
    {
        private readonly DataContractJsonSerializerSettings settings;

        public JsonDataContractSerializer()
        {
            settings = new DataContractJsonSerializerSettings();
        }

        public JsonDataContractSerializer(DataContractJsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public string Serialize<TMessage>(TMessage message)
        {
            DataContractJsonSerializer serailizer = new DataContractJsonSerializer(typeof(TMessage), settings);
            MemoryStream ms = new MemoryStream();
            serailizer.WriteObject(ms, message);
            ms.Position = 0;
            using (StreamReader reader = new StreamReader(ms))
            {
                return reader.ReadToEnd();
            }
        }

        public object Deserialize(Type type, string data)
        {
            DataContractJsonSerializer serailizer = new DataContractJsonSerializer(type, settings);
            MemoryStream ms = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(ms))
            {
                writer.Write(data);
                writer.Flush();
                ms.Position = 0;
                return serailizer.ReadObject(ms);
            }
        }
    }
}
