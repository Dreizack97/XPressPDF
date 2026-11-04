namespace Core.Pdf.ComplementRenderers;

using Core.Abstractions;
using Core.Models.Base;
using Core.Models.Complements.LeyendasFiscales10;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Renderizador modular para la sección del complemento de Leyendas Fiscales 1.0.
/// </summary>
public class LeyendasFiscalesSectionRenderer : ICfdiComplementSectionRenderer
{
    public int Order => 40;

    public bool CanRender(Comprobante comprobante) => comprobante.LeyendasFiscales is not null;

    public void Render(ColumnDescriptor column, Comprobante comprobante)
    {
        LeyendasFiscales? leyendas = comprobante.LeyendasFiscales;
        if (leyendas == null || leyendas.Leyenda.Count == 0)
            return;

        column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
        {
            col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("Leyendas Fiscales").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            foreach (LeyendasFiscalesLeyenda item in leyendas.Leyenda)
            {
                col.Item().Padding(3).Column(itemCol =>
                {
                    if (!string.IsNullOrWhiteSpace(item.disposicionFiscal) || !string.IsNullOrWhiteSpace(item.norma))
                    {
                        itemCol.Item().Text(t =>
                        {
                            if (!string.IsNullOrWhiteSpace(item.disposicionFiscal))
                                t.Span($"Disposición Fiscal: {item.disposicionFiscal} ").Style(CfdiPdfTheme.LabelStyle);
                            if (!string.IsNullOrWhiteSpace(item.norma))
                                t.Span($"| Norma: {item.norma}").Style(CfdiPdfTheme.LabelStyle);
                        });
                    }

                    itemCol.Item().Text(item.textoLeyenda).Style(CfdiPdfTheme.ValueStyle);
                });
            }
        });
    }
}
