using BLL.Interfaces;

namespace BLL.Implementation
{
    public class FileService : IFileService
    {
        public async Task<string> ReadFileAsync(string path)
        {
            return await File.ReadAllTextAsync(path);
        }

        public bool FileExists(string path) => File.Exists(path);

        public bool DirectoryExists(string path) => Directory.Exists(path);
    }
}
