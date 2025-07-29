namespace BLL.Interfaces
{
    public interface IXmlReaderService
    {
        Task<bool> Read(string xmlPath);
    }
}
