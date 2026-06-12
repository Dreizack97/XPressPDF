using BLL.Objects;

namespace BLL.Interfaces
{
    public interface IFtpService : IConnectionTestable
    {
        Task<List<FileUploadResult>> UploadAsync(string filePath, string uploadPath);

        Task<List<FileUploadResult>> UploadAsync(string[] filesPath, string uploadPath);
    }
}
