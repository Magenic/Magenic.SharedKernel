using System.IO;
using System.Xml.Serialization;

namespace Magenic.SharedKernel
{
    /// <summary>
    /// Provides a set of XML helper methods.
    /// </summary>
    public static class XmlEx
    {
        #region Public Static Methods

        /// <summary>
        /// Deserializes XML into object.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <param name="xml">XML content.</param>
        /// <returns>Object of type TSource initialized with XML content.</returns>
        public static TSource Deserialize<TSource>(string xml)
            where TSource : class
        {
            return (new XmlSerializer(typeof(TSource)))
                .WithSideEffectsMap(xmlSerializer => Flow.Using(
                    new StringReader(xml),
                    sr => (TSource)xmlSerializer.Deserialize(sr)));
        }

        /// <summary>
        /// Serializes object into XML and returns string.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <param name="source">Object to be serialized.</param>
        /// <returns>XML.</returns>
        public static string Serialize<TSource>(TSource source)
            where TSource : class
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TSource));

                serializer.Serialize(writer, source);

                return writer.ToString();
            }
        }

        /// <summary>
        /// Serializes object into XML and writes to file.
        /// </summary>
        /// <typeparam name="TSource">Generic data type of the target object of this extension.</typeparam>
        /// <param name="source">Object to be serialized.</param>
        /// <param name="filePath">File path where XML will be written.</param>
        public static void Serialize<TSource>(TSource source, string filePath)
            where TSource : class
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TSource));

                serializer.Serialize(writer, source);
            }
        }

        #endregion
    }
}
