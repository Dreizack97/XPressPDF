namespace BLL
{
    public static class LogManager
    {
        private static string _logDirectory = null!;
        private static readonly object _lock = new object();

        public static void Initialize()
        {
            _logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");

            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }

        public static void Log(string message, string logLevel = "INFO")
        {
            if (string.IsNullOrEmpty(_logDirectory))
                return;

            string logFileName = $"Log-{DateTime.Now:yyyyMMdd}.txt";
            string fullLogPath = Path.Combine(_logDirectory, logFileName);

            lock (_lock)
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
            if (string.IsNullOrEmpty(_logDirectory))
                return;

            string logFileName = $"Log-{DateTime.Now:yyyyMMdd}.txt";
            string fullLogPath = Path.Combine(_logDirectory, logFileName);

            await Task.Run(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        File.AppendAllTextAsync(fullLogPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}] [{logLevel}] {message}{Environment.NewLine}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing to log file {fullLogPath}: {ex.Message}");
                        Console.Error.WriteLine($"Original log message: {message}");
                    }
                }
            });
        }
    }
}
