using System.IO;
using System.Xml.Serialization;

namespace Stations
{
    public static class SerializeExtension
    {
        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));                
                xmlSerializer.Serialize(fs, serializableObject);
            }
        }

        public static T DeserializeObject<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) { return default(T); }
            if (!File.Exists(fileName)) { return default(T); }
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return (T)xmlSerializer.Deserialize(fs);
            }
        }
    }
}
