using System.IO;
using System.Xml.Serialization;

namespace Simulation.Infrastructure
{
    /// <summary>
    /// The XmlSerializerHelper class.
    /// </summary>
    public static class XmlSerializerHelper
    {
        /// <summary>
        /// Reads and returns deserialized object from file
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="filePath">full path to the file</param>
        /// <returns>
        /// Deserialized object.
        /// </returns>
        public static T DeserializeObject<T>(string filePath) where T : class
        {
            object item = null;
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(filePath))
            {
                item = serializer.Deserialize(reader);
            }

            return item as T;
        }
    }
}