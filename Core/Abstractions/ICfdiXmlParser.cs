namespace Core.Abstractions;

using Core.Models.Base;

/// <summary>
/// Contrato para la lectura y deserialización asíncrona de archivos XML CFDI 4.0.
/// </summary>
public interface ICfdiXmlParser
{
    Task<Comprobante> ParseAsync(Stream xmlStream, CancellationToken cancellationToken = default);

    Task<Comprobante> ParseFileAsync(string xmlFilePath, CancellationToken cancellationToken = default);
}
