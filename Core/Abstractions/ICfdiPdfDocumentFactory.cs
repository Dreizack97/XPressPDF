namespace Core.Abstractions;

using Core.Common;
using Core.Models.Base;
using QuestPDF.Infrastructure;

/// <summary>
/// Fábrica responsable de seleccionar y componer el documento QuestPDF adecuado según el tipo de CFDI y complementos.
/// </summary>
public interface ICfdiPdfDocumentFactory
{
    IDocument CreateDocument(Comprobante comprobante, CfdiConversionOptions options);
}
