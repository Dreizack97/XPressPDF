namespace Core.Pdf.Components;

using Core.Formatting;
using Core.Models.Base;
using Core.Models.Complements.TimbreFiscalDigital;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Componente de certificación fiscal: código QR de verificación, datos del Timbre Fiscal Digital, sellos y cadena original.
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
            row.Spacing(6);

            // Código QR (20% del ancho)
            row.RelativeItem(20).Column(col =>
            {
                col.Item().Image(qrBytes).FitArea();
            });

            // Metadatos del Timbre y Criptografía (80% del ancho)
            row.RelativeItem(80).Column(col =>
            {
                col.Spacing(2);

                // Fila 1: Folio Fiscal, No Certificado SAT, Fecha de Certificación
                col.Item().Background(CfdiPdfTheme.LightBackground).CornerRadius(2).PaddingHorizontal(3).PaddingVertical(1).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem().Text("Folio Fiscal (UUID)").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem().Text("No. Certificado SAT").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem().Text("Fecha y Hora Certificación").Style(CfdiPdfTheme.LabelStyle);
                });

                col.Item().PaddingHorizontal(3).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem().Text(tfd?.UUID).Style(CfdiPdfTheme.SmallValueStyle);
                    r.RelativeItem().Text(tfd?.NoCertificadoSAT).Style(CfdiPdfTheme.SmallValueStyle);
                    r.RelativeItem().Text(tfd?.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss")).Style(CfdiPdfTheme.SmallValueStyle);
                });

                // Fila 2: RFC Proveedor y Leyenda
                col.Item().Background(CfdiPdfTheme.LightBackground).CornerRadius(2).PaddingHorizontal(3).PaddingVertical(1).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem().Text("RFC Prov. Certificación:").Style(CfdiPdfTheme.LabelStyle);
                    if (!string.IsNullOrWhiteSpace(tfd?.Leyenda))
                    {
                        r.RelativeItem(2).Text("Leyenda Fiscal:").Style(CfdiPdfTheme.LabelStyle);
                    }
                });

                col.Item().PaddingHorizontal(3).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem().Text(tfd?.RfcProvCertif).Style(CfdiPdfTheme.SmallValueStyle);
                    if (!string.IsNullOrWhiteSpace(tfd?.Leyenda))
                    {
                        r.RelativeItem(2).Text(tfd.Leyenda).Style(CfdiPdfTheme.SmallValueStyle);
                    }
                });

                // Sellos Digitales
                col.Item().Background(CfdiPdfTheme.LightBackground).CornerRadius(2).PaddingHorizontal(3).PaddingVertical(1).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem().Text("Sello Digital del SAT").Style(CfdiPdfTheme.LabelStyle);
                    r.RelativeItem().Text("Sello Digital del CFDI").Style(CfdiPdfTheme.LabelStyle);
                });

                col.Item().PaddingHorizontal(3).Row(r =>
                {
                    r.Spacing(5);
                    r.RelativeItem().Text(tfd?.SelloSAT).Style(CfdiPdfTheme.SealStyle).Justify();
                    r.RelativeItem().Text(tfd?.SelloCFD).Style(CfdiPdfTheme.SealStyle).Justify();
                });

                // Cadena Original del Complemento de Certificación
                col.Item().Background(CfdiPdfTheme.LightBackground).CornerRadius(2).PaddingHorizontal(3).PaddingVertical(1)
                    .Text("Cadena Original del Timbre Fiscal Digital del SAT").Style(CfdiPdfTheme.LabelStyle);

                col.Item().PaddingHorizontal(3)
                    .Text(originalString).Style(CfdiPdfTheme.SealStyle).Justify();
            });
        });
    }
}
