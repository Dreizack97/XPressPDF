namespace Core.Pdf.Components;

using Core.Catalogs;
using Core.Models.Base;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Componente para renderizar la información global de comprobantes expedidos al público en general (&lt;cfdi:InformacionGlobal&gt;).
/// </summary>
public class GlobalInfoComponent : IComponent
{
    private readonly ComprobanteInformacionGlobal _info;

    public GlobalInfoComponent(ComprobanteInformacionGlobal info)
    {
        _info = info;
    }

    public void Compose(IContainer container)
    {
        container.Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(column =>
        {
            column.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("Información Global (Ventas al Público en General)").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            column.Item().PaddingHorizontal(5).PaddingVertical(2.5F).Row(row =>
            {
                row.Spacing(10);
                row.RelativeItem().Row(r =>
                {
                    r.AutoItem().Text("Periodicidad: ").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem().Text(SatCatalogProvider.GetPeriodicidad(_info.Periodicidad)).Style(CfdiPdfTheme.ValueStyle);
                });

                row.RelativeItem().Row(r =>
                {
                    r.AutoItem().Text("Meses: ").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem().Text(SatCatalogProvider.GetMeses(_info.Meses)).Style(CfdiPdfTheme.ValueStyle);
                });

                row.RelativeItem().Row(r =>
                {
                    r.AutoItem().Text("Año: ").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem().Text(_info.Año.ToString()).Style(CfdiPdfTheme.ValueStyle);
                });
            });
        });
    }
}
