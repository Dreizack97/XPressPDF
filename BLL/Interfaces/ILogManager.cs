namespace BLL.Interfaces
{
    public interface ILogManager
    {
        void Log(string message, string logLevel = "INFO");

        Task LogAsync(string message, string logLevel = "INFO");
    }
}
