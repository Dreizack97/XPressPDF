namespace Core.Pdf.Documents;

using Core.Common;
using Core.Models.Base;
using Core.Pdf.Components;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

/// <summary>
/// Plantilla base (Template Method) para las representaciones impresas de CFDI 4.0.
/// Centraliza la maquetación de página, encabezado, pie reglamentario del SAT y numeración.
/// </summary>
public abstract class CfdiDocumentBase : IDocument
{
    public Comprobante Comprobante { get; }

    public CfdiConversionOptions Options { get; }

    protected CfdiDocumentBase(Comprobante comprobante, CfdiConversionOptions options)
    {
        Comprobante = comprobante;
        Options = options;
    }

    protected abstract string DocumentTitle { get; }

    public DocumentMetadata GetMetadata() => new()
    {
        Title = DocumentTitle,
        Author = Comprobante.Emisor?.Nombre ?? "XPressPDF",
        Subject = $"CFDI 4.0 - Folio Fiscal: {Comprobante.TimbreFiscalDigital?.UUID}",
        Creator = "XPressPDF .NET 10 Engine",
        Producer = "QuestPDF"
    };

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.Letter);
            page.Margin(1.2F, Unit.Centimetre);

            page.Content().Column(column =>
            {
                column.Item().Component(new HeaderComponent(Comprobante, DocumentTitle, Options));
                column.Item().Element(ComposeContent);
                column.Item().Component(new TfdFooterComponent(Comprobante));
            });

            page.Footer().Row(row =>
            {
                row.RelativeItem().Text("Este documento es una representación impresa de un CFDI").Style(CfdiPdfTheme.SmallValueStyle);

                row.AutoItem().Text(text =>
                {
                    text.DefaultTextStyle(CfdiPdfTheme.SmallValueStyle);
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        });
    }

    protected abstract void ComposeContent(IContainer container);

    protected string FormatMoney(decimal amount) => amount.ToString("C", Options.Culture);
}
