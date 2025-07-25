using BLL.Interfaces;
using System.Xml.Serialization;

namespace BLL.Implementation
{
    public class XmlDeserializer : IXmlDeserializer
    {
        public T? Deserialize<T>(string xmlContent) where T : class
        {
            try
            {
                using (StringReader stringReader = new StringReader(xmlContent))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    return serializer.Deserialize(stringReader) as T;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
