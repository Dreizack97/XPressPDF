namespace Core.Services;

using Core.Abstractions;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Servicio de bitácora y registro estructurado en disco para diagnóstico y trazabilidad de conversiones.
/// </summary>
public class FileLogService : ILogService
{
    private readonly string _logDirectory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public FileLogService(string? logDirectory = null)
    {
        _logDirectory = logDirectory ?? Path.Combine(AppContext.BaseDirectory, "logs");
    }

    public void Log(string message, string level = "INFO")
    {
        try
        {
            EnsureDirectory();
            string logFile = GetLogFilePath();
            string entry = FormatLogEntry(message, level);

            lock (_semaphore)
            {
                File.AppendAllText(logFile, entry, Encoding.UTF8);
            }
        }
        catch
        {
            // Evitar fallas catastróficas por error de escritura de log
        }
    }

    public async Task LogAsync(string message, string level = "INFO", CancellationToken cancellationToken = default)
    {
        try
        {
            EnsureDirectory();
            string logFile = GetLogFilePath();
            string entry = FormatLogEntry(message, level);

            await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                await File.AppendAllTextAsync(logFile, entry, Encoding.UTF8, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        catch
        {
            // Evitar fallas catastróficas por error de escritura de log
        }
    }

    private void EnsureDirectory()
    {
        if (!Directory.Exists(_logDirectory))
        {
            Directory.CreateDirectory(_logDirectory);
        }
    }

    private string GetLogFilePath() =>
        Path.Combine(_logDirectory, $"xpresspdf_{DateTime.Now:yyyyMMdd}.log");

    private static string FormatLogEntry(string message, string level) =>
        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level.ToUpperInvariant()}] {message}{Environment.NewLine}";
}
