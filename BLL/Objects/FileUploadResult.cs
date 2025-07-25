namespace BLL.Objetcs
{
    public class FileUploadResult
    {
        public string FilePath { get; }

        public string RemotePath { get; }

        public bool Success { get; }

        public string? ErrorMessage { get; }

        public FileUploadResult(string filePath, string remotePath, bool success, string? errorMessage = null)
        {
            FilePath = filePath;
            RemotePath = remotePath;
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
