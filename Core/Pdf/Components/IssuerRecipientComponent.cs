namespace Core.Pdf.Components;

using Core.Catalogs;
using Core.Models.Base;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Componente de dos columnas con la información fiscal del Emisor y Receptor con alturas simétricas.
/// </summary>
public class IssuerRecipientComponent : IComponent
{
    private readonly Comprobante _comprobante;

    public IssuerRecipientComponent(Comprobante comprobante)
    {
        _comprobante = comprobante;
    }

    public void Compose(IContainer container)
    {
        container.Row(row =>
        {
            row.Spacing(10);

            // Datos del Emisor
            row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
            {
                col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                    .Text("Datos del Emisor").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                col.Item().Padding(4).Column(content =>
                {
                    content.Spacing(1);
                    content.Item().Text(_comprobante.Emisor.Nombre).Style(CfdiPdfTheme.LabelStyle);
                    LabeledRow(content, "RFC:", _comprobante.Emisor.Rfc);
                    LabeledRow(content, "Régimen Fiscal:", SatCatalogProvider.GetRegimenFiscal(_comprobante.Emisor.RegimenFiscal));
                    LabeledRow(content, "No. Certificado:", _comprobante.NoCertificado);
                });
            });

            // Datos del Receptor
            row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
            {
                col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                    .Text("Datos del Receptor").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                col.Item().Padding(4).Column(content =>
                {
                    content.Spacing(1);
                    content.Item().Text(_comprobante.Receptor.Nombre).Style(CfdiPdfTheme.LabelStyle);
                    LabeledRow(content, "RFC:", _comprobante.Receptor.Rfc);
                    LabeledRow(content, "Domicilio Fiscal (C.P.):", _comprobante.Receptor.DomicilioFiscalReceptor);
                    LabeledRow(content, "Régimen Fiscal:", SatCatalogProvider.GetRegimenFiscal(_comprobante.Receptor.RegimenFiscalReceptor));
                    LabeledRow(content, "Uso del CFDI:", SatCatalogProvider.GetUsoCfdi(_comprobante.Receptor.UsoCFDI));

                    if (!string.IsNullOrWhiteSpace(_comprobante.Receptor.ResidenciaFiscal))
                    {
                        LabeledRow(content, "Residencia Fiscal:", _comprobante.Receptor.ResidenciaFiscal);
                    }

                    if (!string.IsNullOrWhiteSpace(_comprobante.Receptor.NumRegIdTrib))
                    {
                        LabeledRow(content, "NumRegIdTrib:", _comprobante.Receptor.NumRegIdTrib);
                    }
                });
            });
        });
    }

    private static void LabeledRow(ColumnDescriptor col, string label, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        col.Item().Row(row =>
        {
            row.Spacing(2);
            row.AutoItem().Text(label).Style(CfdiPdfTheme.LabelStyle);
            row.RelativeItem().Text(value).Style(CfdiPdfTheme.ValueStyle);
        });
    }
}
