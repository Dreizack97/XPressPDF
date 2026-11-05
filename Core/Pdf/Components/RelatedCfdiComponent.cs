namespace Core.Pdf.Components;

using Core.Catalogs;
using Core.Models.Base;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Componente para renderizar la sección de comprobantes fiscales relacionados (&lt;cfdi:CfdiRelacionados&gt;).
/// </summary>
public class RelatedCfdiComponent : IComponent
{
    private readonly ComprobanteCfdiRelacionados[] _relatedList;

    public RelatedCfdiComponent(ComprobanteCfdiRelacionados[] relatedList)
    {
        _relatedList = relatedList;
    }

    public void Compose(IContainer container)
    {
        if (_relatedList.Length == 0)
            return;

        container.Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(column =>
        {
            column.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("CFDI Relacionados").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            foreach (ComprobanteCfdiRelacionados group in _relatedList)
            {
                column.Item().Background(CfdiPdfTheme.LightBackground).PaddingHorizontal(6).PaddingVertical(2).Text(t =>
                {
                    t.Span("Tipo de Relación: ").Style(CfdiPdfTheme.LabelStyle);
                    t.Span(SatCatalogProvider.GetTipoRelacion(group.TipoRelacion)).Style(CfdiPdfTheme.ValueStyle);
                });

                foreach (ComprobanteCfdiRelacionadosCfdiRelacionado rel in group.CfdiRelacionado)
                {
                    column.Item().PaddingHorizontal(8).PaddingVertical(1.5F).Text(t =>
                    {
                        t.Span("• UUID Relacionado: ").Style(CfdiPdfTheme.LabelStyle);
                        t.Span(rel.UUID).Style(CfdiPdfTheme.ValueStyle);
                    });
                }
            }
        });
    }
}
