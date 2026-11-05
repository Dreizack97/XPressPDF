namespace Core.Pdf;

using Core.Abstractions;
using Core.Common;
using Core.Models.Base;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Motor principal de generación de documentos PDF para CFDI 4.0 utilizando QuestPDF.
/// </summary>
public class CfdiPdfGenerator : ICfdiPdfGenerator
{
    static CfdiPdfGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    private readonly ICfdiPdfDocumentFactory _documentFactory;

    public CfdiPdfGenerator(ICfdiPdfDocumentFactory documentFactory)
    {
        _documentFactory = documentFactory;
    }

    public byte[] GeneratePdfBytes(Comprobante comprobante, CfdiConversionOptions? options = null)
    {
        options ??= new CfdiConversionOptions();
        IDocument document = _documentFactory.CreateDocument(comprobante, options);
        return document.GeneratePdf();
    }

    public void GeneratePdfStream(Comprobante comprobante, Stream outputStream, CfdiConversionOptions? options = null)
    {
        options ??= new CfdiConversionOptions();
        IDocument document = _documentFactory.CreateDocument(comprobante, options);
        document.GeneratePdf(outputStream);
    }

    public async Task GeneratePdfFileAsync(
        Comprobante comprobante,
        string outputPdfPath,
        CfdiConversionOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        options ??= new CfdiConversionOptions();
        IDocument document = _documentFactory.CreateDocument(comprobante, options);

        string? directory = Path.GetDirectoryName(outputPdfPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        byte[] pdfBytes = document.GeneratePdf();
        await File.WriteAllBytesAsync(outputPdfPath, pdfBytes, cancellationToken).ConfigureAwait(false);
    }
}
