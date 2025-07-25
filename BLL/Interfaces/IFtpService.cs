using BLL.Objetcs;

namespace BLL.Interfaces
{
    public interface IFtpService
    {
        List<FileUploadResult> Upload(string filePath, string uploadPath);

        List<FileUploadResult> Upload(string[] filesPath, string uploadPath);
    }
}
