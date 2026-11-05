namespace Core.Pdf.Components;

using Core.Catalogs;
using Core.Common;
using Core.Models.Base;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Componente de encabezado: logotipo corporativo y recuadro oficial de datos generales de emisión.
/// </summary>
public class HeaderComponent : IComponent
{
    private readonly Comprobante _comprobante;
    private readonly string _documentTitle;
    private readonly CfdiConversionOptions _options;

    public HeaderComponent(Comprobante comprobante, string documentTitle, CfdiConversionOptions options)
    {
        _comprobante = comprobante;
        _documentTitle = documentTitle;
        _options = options;
    }

    public void Compose(IContainer container)
    {
        container.Row(row =>
        {
            row.Spacing(10);

            // Columna de Logotipo
            if (_options.LogoImageBytes is { Length: > 0 } bytes)
            {
                row.RelativeItem().AlignLeft().AlignMiddle().MaxHeight(50).Image(bytes).FitArea();
            }
            else if (!string.IsNullOrWhiteSpace(_options.LogoImagePath) && File.Exists(_options.LogoImagePath))
            {
                row.RelativeItem().AlignLeft().AlignMiddle().MaxHeight(50).Image(_options.LogoImagePath).FitArea();
            }
            else
            {
                row.RelativeItem();
            }

            // Recuadro de Serie, Folio, Lugar y Fecha
            row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(column =>
            {
                column.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(3)
                    .Text(_documentTitle).Style(CfdiPdfTheme.TitleStyle).AlignCenter();

                column.Item().Background(CfdiPdfTheme.LightBackground).PaddingHorizontal(5).PaddingVertical(2).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem(20).Text("Serie").Style(CfdiPdfTheme.HeaderLabelStyle);
                    r.RelativeItem(25).Text("Folio").Style(CfdiPdfTheme.HeaderLabelStyle);
                    r.RelativeItem(25).Text("Lugar").Style(CfdiPdfTheme.HeaderLabelStyle);
                    r.RelativeItem(30).Text("Fecha").Style(CfdiPdfTheme.HeaderLabelStyle).AlignEnd();
                });

                column.Item().PaddingHorizontal(5).PaddingVertical(2).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem(20).Text(_comprobante.Serie).Style(CfdiPdfTheme.ValueStyle);
                    r.RelativeItem(25).Text(_comprobante.Folio).Style(CfdiPdfTheme.ValueStyle);
                    r.RelativeItem(25).Text(_comprobante.LugarExpedicion).Style(CfdiPdfTheme.ValueStyle);
                    r.RelativeItem(30).Text(_comprobante.Fecha.ToString("yyyy-MM-ddTHH:mm:ss")).Style(CfdiPdfTheme.ValueStyle).AlignEnd();
                });
            });
        });
    }
}
