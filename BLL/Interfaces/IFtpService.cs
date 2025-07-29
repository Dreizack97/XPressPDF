using BLL.Objetcs;

namespace BLL.Interfaces
{
    public interface IFtpService
    {
        Task<bool> ConnectAsync();

        Task<List<FileUploadResult>> UploadAsync(string filePath, string uploadPath);

        Task<List<FileUploadResult>> UploadAsync(string[] filesPath, string uploadPath);
    }
}
