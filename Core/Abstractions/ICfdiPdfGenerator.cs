namespace Core.Abstractions;

using Core.Common;
using Core.Models.Base;

/// <summary>
/// Contrato del motor de generación de representaciones impresas (PDF) de CFDI 4.0.
/// </summary>
public interface ICfdiPdfGenerator
{
    byte[] GeneratePdfBytes(Comprobante comprobante, CfdiConversionOptions? options = null);

    void GeneratePdfStream(Comprobante comprobante, Stream outputStream, CfdiConversionOptions? options = null);

    Task GeneratePdfFileAsync(Comprobante comprobante, string outputPdfPath, CfdiConversionOptions? options = null, CancellationToken cancellationToken = default);
}
