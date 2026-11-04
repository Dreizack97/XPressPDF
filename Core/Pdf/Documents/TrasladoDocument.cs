namespace Core.Pdf.Documents;

using Core.Abstractions;
using Core.Common;
using Core.Models.Base;
using Core.Pdf.Components;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Representación impresa de CFDI de Traslado (Tipo T).
/// </summary>
public class TrasladoDocument : CfdiDocumentBase
{
    private readonly IEnumerable<ICfdiComplementSectionRenderer> _complementRenderers;

    public TrasladoDocument(
        Comprobante comprobante,
        CfdiConversionOptions options,
        IEnumerable<ICfdiComplementSectionRenderer>? complementRenderers = null)
        : base(comprobante, options)
    {
        _complementRenderers = (complementRenderers ?? []).OrderBy(r => r.Order);
    }

    protected override string DocumentTitle => "CFDI de Traslado";

    protected override void ComposeContent(IContainer container)
    {
        container.PaddingVertical(6).Column(column =>
        {
            column.Spacing(6);

            // 1. Emisor y Receptor
            column.Item().Component(new IssuerRecipientComponent(Comprobante));

            // 2. CFDI Relacionados
            if (Comprobante.CfdiRelacionados is { Length: > 0 })
            {
                column.Item().Component(new RelatedCfdiComponent(Comprobante.CfdiRelacionados));
            }

            // 3. Conceptos amparados en el traslado
            column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
            {
                col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                    .Text("Mercancías / Bienes en Traslado").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(60);   // Clave SAT
                        columns.ConstantColumn(50);   // Cantidad
                        columns.ConstantColumn(45);   // Clave Unidad
                        columns.RelativeColumn(1.0F); // Unidad
                        columns.RelativeColumn(4.0F); // Descripción
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Clave SAT").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Cant.").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Clave").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Unidad").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Descripción").Style(CfdiPdfTheme.LabelStyle);
                    });

                    foreach (ComprobanteConcepto concepto in Comprobante.Conceptos)
                    {
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.ClaveProdServ).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.Cantidad.ToString("0.##")).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.ClaveUnidad).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.Unidad).Style(CfdiPdfTheme.ValueStyle);

                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Column(descCol =>
                        {
                            descCol.Item().Text(concepto.Descripcion).Style(CfdiPdfTheme.ValueStyle);
                            if (!string.IsNullOrWhiteSpace(concepto.NoIdentificacion))
                            {
                                descCol.Item().Text($"No. Identificación: {concepto.NoIdentificacion}").Style(CfdiPdfTheme.SmallValueStyle);
                            }
                        });
                    }
                });
            });

            // 4. Secciones de complementos modulares (Carta Porte, Leyendas Fiscales, etc.)
            foreach (ICfdiComplementSectionRenderer renderer in _complementRenderers)
            {
                if (renderer.CanRender(Comprobante))
                {
                    renderer.Render(column, Comprobante);
                }
            }
        });
    }
}
