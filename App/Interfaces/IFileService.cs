namespace App.Interfaces
{
    public interface IFileService
    {
        Task<string> ReadFileAsync(string path);

        bool FileExists(string path);
    }
}
