namespace Core.Abstractions;

/// <summary>
/// Servicio abstracto para el registro de eventos y diagnóstico del motor.
/// </summary>
public interface ILogService
{
    void Log(string message, string level = "INFO");

    Task LogAsync(string message, string level = "INFO", CancellationToken cancellationToken = default);
}
