namespace Core.Common;

/// <summary>
/// Resultado inmutable de la conversión de un archivo XML CFDI a PDF.
/// </summary>
public record CfdiConversionResult(
    string SourceFilePath,
    string? OutputPdfPath,
    string? Uuid,
    bool Success,
    string? ErrorMessage,
    TimeSpan Duration);
