namespace Core.Pdf.ComplementRenderers;

using Core.Abstractions;
using Core.Models.Base;
using Core.Models.Complements.ImpuestosLocales10;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

/// <summary>
/// Renderizador modular para la sección del complemento de Impuestos Locales 1.0.
/// </summary>
public class ImpuestosLocalesSectionRenderer : ICfdiComplementSectionRenderer
{
    private static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("es-MX");

    public int Order => 30;

    public bool CanRender(Comprobante comprobante) => comprobante.ImpuestosLocales is not null;

    public void Render(ColumnDescriptor column, Comprobante comprobante)
    {
        ImpuestosLocales? local = comprobante.ImpuestosLocales;
        if (local == null)
            return;

        column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
        {
            col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("Complemento de Impuestos Locales").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            if (local.TrasladosLocales.Count > 0)
            {
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Impuesto
                        columns.RelativeColumn(1); // Tasa
                        columns.RelativeColumn(1.5F); // Importe
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Traslado Local").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tasa").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Importe").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                    });

                    foreach (ImpuestosLocalesTrasladoLocal traslado in local.TrasladosLocales)
                    {
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(traslado.ImpLocTrasladado).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text($"{traslado.TasadeTraslado:N2}%").Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(traslado.Importe.ToString("C", Culture)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    }
                });
            }

            if (local.RetencionesLocales.Count > 0)
            {
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Impuesto
                        columns.RelativeColumn(1); // Tasa
                        columns.RelativeColumn(1.5F); // Importe
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Retención Local").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tasa").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Importe").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                    });

                    foreach (ImpuestosLocalesRetencionLocal retencion in local.RetencionesLocales)
                    {
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(retencion.ImpLocRetenido).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text($"{retencion.TasadeRetencion:N2}%").Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(retencion.Importe.ToString("C", Culture)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    }
                });
            }

            // Resumen de totales locales
            col.Item().Padding(4).Row(row =>
            {
                row.RelativeItem().Text($"Total de Traslados Locales: {local.TotaldeTraslados.ToString("C", Culture)}").Style(CfdiPdfTheme.LabelStyle);
                row.RelativeItem().Text($"Total de Retenciones Locales: {local.TotaldeRetenciones.ToString("C", Culture)}").Style(CfdiPdfTheme.LabelStyle).AlignRight();
            });
        });
    }
}
