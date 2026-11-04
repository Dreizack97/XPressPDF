namespace Core.Common;

/// <summary>
/// Reporte de progreso durante el procesamiento de conversiones por lotes.
/// </summary>
public record CfdiConversionProgress(
    int ProcessedCount,
    int TotalCount,
    string CurrentFile,
    bool CurrentSuccess);
