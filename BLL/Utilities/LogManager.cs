namespace BLL.Utilities
{
    public static class LogManager
    {
        private static string _logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public static void Initialize()
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }

        public static void Log(string message, string logLevel = "INFO")
        {
            Initialize();

            string logFileName = $"Log-{DateTime.Now:yyyyMMdd}.txt";
            string fullLogPath = Path.Combine(_logDirectory, logFileName);

            lock (_semaphore)
            {
                try
                {
                    File.AppendAllText(fullLogPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [{logLevel}] {message}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error writing to log file {fullLogPath}: {ex.Message}");
                    Console.Error.WriteLine($"Original log message: {message}");
                }
            }
        }

        public static async Task LogAsync(string message, string logLevel = "INFO")
        {
            Initialize();

            string logFileName = $"Log-{DateTime.Now:yyyyMMdd}.txt";
            string fullLogPath = Path.Combine(_logDirectory, logFileName);

            await _semaphore.WaitAsync();

            try
            {
                await File.AppendAllTextAsync(fullLogPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [{logLevel}] {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing to log file {fullLogPath}: {ex.Message}");
                Console.Error.WriteLine($"Original log message: {message}");
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
