using App.Interfaces;

namespace App.Implementation
{
    public class FileService : IFileService
    {
        public async Task<string> ReadFileAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }

        public bool FileExists(string path) => File.Exists(path);
    }
}
