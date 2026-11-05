namespace Core.Pdf.ComplementRenderers;

using Core.Abstractions;
using Core.Models.Base;
using Core.Models.Complements.ComercioExterior20;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Renderizador modular para la sección del complemento de Comercio Exterior 2.0.
/// </summary>
public class ComercioExteriorSectionRenderer : ICfdiComplementSectionRenderer
{
    public int Order => 60;

    public bool CanRender(Comprobante comprobante) => comprobante.ComercioExterior is not null;

    public void Render(ColumnDescriptor column, Comprobante comprobante)
    {
        ComercioExterior? cce = comprobante.ComercioExterior;
        if (cce == null)
            return;

        column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
        {
            col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("Complemento de Comercio Exterior 2.0").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            col.Item().Padding(3).Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text(t => { t.Span("Clave de Pedimento: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cce.ClaveDePedimento).Style(CfdiPdfTheme.ValueStyle); });
                    c.Item().Text(t => { t.Span("Certificado Origen: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cce.CertificadoOrigen == 1 ? "Sí" : "No").Style(CfdiPdfTheme.ValueStyle); });
                    if (!string.IsNullOrWhiteSpace(cce.Incoterm))
                        c.Item().Text(t => { t.Span("Incoterm: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cce.Incoterm).Style(CfdiPdfTheme.ValueStyle); });
                });

                row.RelativeItem().Column(c =>
                {
                    if (cce.TipoCambioUSDSpecified)
                        c.Item().Text(t => { t.Span("Tipo de Cambio USD: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cce.TipoCambioUSD.ToString("N4")).Style(CfdiPdfTheme.ValueStyle); });
                    if (cce.TotalUSDSpecified)
                        c.Item().Text(t => { t.Span("Total USD: ").Style(CfdiPdfTheme.LabelStyle); t.Span($"$ {cce.TotalUSD:N2} USD").Style(CfdiPdfTheme.ValueStyle); });
                    if (!string.IsNullOrWhiteSpace(cce.NumeroExportadorConfiable))
                        c.Item().Text(t => { t.Span("No. Exportador Confiable: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cce.NumeroExportadorConfiable).Style(CfdiPdfTheme.ValueStyle); });
                });
            });

            if (cce.Mercancias.Count > 0)
            {
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // No Identificación
                        columns.RelativeColumn(2); // Fracción Arancelaria
                        columns.RelativeColumn(1.5F); // Cantidad Aduana
                        columns.RelativeColumn(1.5F); // Unidad
                        columns.RelativeColumn(1.5F); // Valor Unitario
                        columns.RelativeColumn(1.5F); // Valor Dólares
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("No. Identificación").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Fracc. Arancelaria").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Cantidad").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Unidad").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Valor Unitario").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Valor Dólares").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                    });

                    foreach (ComercioExteriorMercancia item in cce.Mercancias)
                    {
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(item.NoIdentificacion).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(item.FraccionArancelaria ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(item.CantidadAduanaSpecified ? item.CantidadAduana.ToString("N2") : "-").Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(item.UnidadAduana ?? "-").Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(item.ValorUnitarioAduanaSpecified ? $"${item.ValorUnitarioAduana:N2}" : "-").Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text($"${item.ValorDolares:N2}").Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    }
                });
            }
        });
    }
}
