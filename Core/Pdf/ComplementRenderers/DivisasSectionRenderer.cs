namespace Core.Pdf.ComplementRenderers;

using Core.Abstractions;
using Core.Models.Base;
using Core.Models.Complements.Divisas10;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Renderizador modular para la sección del complemento de Operaciones con Divisas 1.0.
/// </summary>
public class DivisasSectionRenderer : ICfdiComplementSectionRenderer
{
    public int Order => 50;

    public bool CanRender(Comprobante comprobante) => comprobante.Divisas is not null;

    public void Render(ColumnDescriptor column, Comprobante comprobante)
    {
        Divisas? divisas = comprobante.Divisas;
        if (divisas == null)
            return;

        column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
        {
            col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("Complemento de Compra y Venta de Divisas").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            col.Item().Padding(4).Row(row =>
            {
                row.RelativeItem().Text(t =>
                {
                    t.Span("Tipo de Operación: ").Style(CfdiPdfTheme.LabelStyle);
                    t.Span(divisas.tipoOperacion.ToUpperInvariant()).Style(CfdiPdfTheme.ValueBoldStyle);
                });
            });
        });
    }
}
