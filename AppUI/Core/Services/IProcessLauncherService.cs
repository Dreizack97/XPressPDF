namespace AppUI.Core.Services;

public interface IProcessLauncherService
{
    bool OpenFile(string filePath);
    bool OpenDirectory(string directoryPath);
    bool OpenDirectoryAndSelect(string filePath);
}
