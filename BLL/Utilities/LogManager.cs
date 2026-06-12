using BLL.Interfaces;

namespace BLL.Utilities
{
    public class LogManager : ILogManager
    {
        private readonly string _logDirectory;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public LogManager()
            : this(Path.Combine(AppContext.BaseDirectory, "Logs"))
        {
        }

        public LogManager(string logDirectory)
        {
            _logDirectory = logDirectory;
        }

        public void Log(string message, string logLevel = "INFO")
        {
            string fullLogPath = GetLogFilePath();

            _semaphore.Wait();

            try
            {
                File.AppendAllText(fullLogPath, FormatEntry(message, logLevel));
            }
            catch (Exception ex)
            {
                ReportFailure(fullLogPath, message, ex);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task LogAsync(string message, string logLevel = "INFO")
        {
            string fullLogPath = GetLogFilePath();

            await _semaphore.WaitAsync();

            try
            {
                await File.AppendAllTextAsync(fullLogPath, FormatEntry(message, logLevel));
            }
            catch (Exception ex)
            {
                ReportFailure(fullLogPath, message, ex);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private string GetLogFilePath()
        {
            Directory.CreateDirectory(_logDirectory);

            return Path.Combine(_logDirectory, $"Log-{DateTime.Now:yyyyMMdd}.txt");
        }

        private static string FormatEntry(string message, string logLevel) =>
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [{logLevel}] {message}{Environment.NewLine}";

        private static void ReportFailure(string logPath, string message, Exception ex)
        {
            Console.Error.WriteLine($"Error writing to log file {logPath}: {ex.Message}");
            Console.Error.WriteLine($"Original log message: {message}");
        }
    }
}
