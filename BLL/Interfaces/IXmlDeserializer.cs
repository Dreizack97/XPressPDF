namespace BLL.Interfaces
{
    public interface IXmlDeserializer
    {
        T? Deserialize<T>(string xmlContent) where T : class;
    }
}
