namespace Core.Pdf.Components;

using Core.Formatting;
using Core.Models.Base;
using Core.Models.Complements.TimbreFiscalDigital;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Componente de certificación fiscal: código QR de verificación responsivo, datos del Timbre Fiscal Digital, sellos y cadena original.
/// </summary>
public class TfdFooterComponent : IComponent
{
    private readonly Comprobante _comprobante;

    public TfdFooterComponent(Comprobante comprobante)
    {
        _comprobante = comprobante;
    }

    public void Compose(IContainer container)
    {
        TimbreFiscalDigital? tfd = _comprobante.TimbreFiscalDigital;
        string verificationUrl = SatVerificationUrlBuilder.Build(_comprobante);
        byte[] qrBytes = SatQrCodeGenerator.GeneratePngBytes(verificationUrl);
        string originalString = TfdOriginalStringBuilder.Build(tfd);

        container.PaddingTop(6).ShowEntire().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Padding(4).Row(row =>
        {
            row.Spacing(8);

            // Código QR responsivo ajustado dinámicamente a la altura de los metadatos
            row.RelativeItem(24).AlignMiddle().Image(qrBytes).FitArea();

            // Metadatos del Timbre y Criptografía (76% del ancho)
            row.RelativeItem(76).Column(col =>
            {
                col.Spacing(2);

                // Fila 1: Folio Fiscal (UUID), No. Certificado SAT, Fecha y Hora Certificación
                col.Item().Background(CfdiPdfTheme.LightBackground).CornerRadius(2).PaddingHorizontal(4).PaddingVertical(1.5F).Row(r =>
                {
                    r.Spacing(6);
                    r.RelativeItem(45).Text("Folio Fiscal (UUID)").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem(28).Text("No. Certificado SAT").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem(27).Text("Fecha y Hora Certificación").Style(CfdiPdfTheme.LabelStyle);
                });

                col.Item().PaddingHorizontal(4).Row(r =>
                {
                    r.Spacing(6);
                    r.RelativeItem(45).Text(tfd?.UUID).Style(CfdiPdfTheme.SmallValueStyle);
                    r.RelativeItem(28).Text(tfd?.NoCertificadoSAT).Style(CfdiPdfTheme.SmallValueStyle);
                    r.RelativeItem(27).Text(tfd?.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss")).Style(CfdiPdfTheme.SmallValueStyle);
                });

                // Fila 2: RFC Proveedor de Certificación y Leyenda Fiscal
                col.Item().PaddingHorizontal(4).Text(t =>
                {
                    t.Span("RFC Prov. Certificación: ").Style(CfdiPdfTheme.LabelStyle);
                    t.Span(tfd?.RfcProvCertif ?? "N/A").Style(CfdiPdfTheme.SmallValueStyle);

                    if (!string.IsNullOrWhiteSpace(tfd?.Leyenda))
                    {
                        t.Span("  |  Leyenda Fiscal: ").Style(CfdiPdfTheme.LabelStyle);
                        t.Span(tfd.Leyenda).Style(CfdiPdfTheme.SmallValueStyle);
                    }
                });

                // Fila 3: Sellos Digitales del SAT y del CFDI
                col.Item().Background(CfdiPdfTheme.LightBackground).CornerRadius(2).PaddingHorizontal(4).PaddingVertical(1.5F).Row(r =>
                {
                    r.Spacing(6);
                    r.RelativeItem().Text("Sello Digital del SAT").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem().Text("Sello Digital del CFDI").Style(CfdiPdfTheme.LabelStyle);
                });

                col.Item().PaddingHorizontal(4).Row(r =>
                {
                    r.Spacing(6);
                    r.RelativeItem().Text(tfd?.SelloSAT).Style(CfdiPdfTheme.SealStyle).Justify();
                    r.RelativeItem().Text(tfd?.SelloCFD).Style(CfdiPdfTheme.SealStyle).Justify();
                });

                // Fila 4: Cadena Original del Timbre Fiscal Digital del SAT
                col.Item().Background(CfdiPdfTheme.LightBackground).CornerRadius(2).PaddingHorizontal(4).PaddingVertical(1.5F)
                    .Text("Cadena Original del Timbre Fiscal Digital del SAT").Style(CfdiPdfTheme.LabelStyle);

                col.Item().PaddingHorizontal(4)
                    .Text(originalString).Style(CfdiPdfTheme.SealStyle).Justify();
            });
        });
    }
}
