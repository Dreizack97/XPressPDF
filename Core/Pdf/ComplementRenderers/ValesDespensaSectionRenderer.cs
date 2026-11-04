namespace Core.Pdf.ComplementRenderers;

using Core.Abstractions;
using Core.Models.Base;
using Core.Models.Complements.ValesDeDespensa10;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Renderizador modular para la sección del complemento de Vales de Despensa 1.0.
/// </summary>
public class ValesDespensaSectionRenderer : ICfdiComplementSectionRenderer
{
    public int Order => 20;

    public bool CanRender(Comprobante comprobante) => comprobante.ValesDespensa is not null;

    public void Render(ColumnDescriptor column, Comprobante comprobante)
    {
        ValesDeDespensa? vales = comprobante.ValesDespensa;
        if (vales == null)
            return;

        // Encabezado del complemento
        column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
        {
            col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("Complemento de Vales de Despensa").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1.5F);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tipo de Operación").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Registro Patronal").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Número de Cuenta").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Total Dispersado").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                });

                table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(vales.TipoOperacion).Style(CfdiPdfTheme.ValueStyle);
                table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(vales.RegistroPatronal ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
                table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(vales.NumeroDeCuenta).Style(CfdiPdfTheme.ValueStyle);
                table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(vales.Total.ToString("C")).Style(CfdiPdfTheme.ValueStyle).AlignRight();
            });
        });

        // Tabla de Dispersión por Beneficiario
        ValesDeDespensaConceptosConcepto[] conceptos = vales.Conceptos?.Concepto ?? [];
        if (conceptos.Length == 0)
            return;

        column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
        {
            col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text($"Dispersión de Vales de Despensa ({conceptos.Length} beneficiarios)").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1.1F); // Identificador
                    columns.RelativeColumn(0.9F); // Fecha
                    columns.RelativeColumn(1.3F); // RFC
                    columns.RelativeColumn(1.7F); // CURP
                    columns.RelativeColumn(2.2F); // Nombre
                    columns.RelativeColumn(1.0F); // Importe
                });

                table.Header(header =>
                {
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Identificador").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Fecha").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("RFC").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("CURP").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Nombre").Style(CfdiPdfTheme.LabelStyle);
                    header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Importe").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                });

                var ordenados = conceptos.OrderBy(c => c.nombre?.Trim(), StringComparer.OrdinalIgnoreCase);

                foreach (ValesDeDespensaConceptosConcepto concepto in ordenados)
                {
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.identificador).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.fecha.ToString("yyyy-MM-dd")).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.rfc).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.curp).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.nombre?.Trim()).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.importe.ToString("C")).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                }

                table.Footer(footer =>
                {
                    footer.Cell().ColumnSpan(5).Element(CfdiPdfTheme.CellHeaderStyle).Text("Total Dispersión").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                    footer.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text(vales.Total.ToString("C")).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                });
            });
        });
    }
}
