using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Schemas.Base;
using System.Globalization;

namespace BLL.Utilities
{
    /// <summary>
    /// Base para toda representación impresa de un CFDI. Centraliza el encabezado (serie, folio,
    /// lugar y fecha de expedición), el pie con los datos del Timbre Fiscal Digital, código QR de
    /// verificación del SAT y la cadena original del complemento, además de los estilos tipográficos,
    /// garantizando consistencia visual entre los distintos tipos de comprobante.
    /// </summary>
    public abstract class CfdiDocumentBase : IDocument
    {
        protected static readonly CultureInfo MxCulture = CultureInfo.GetCultureInfo("es-MX");

        protected static readonly TextStyle TitleStyle = TextStyle.Default.FontFamily(Fonts.Arial).FontSize(10).FontColor(Colors.Grey.Darken3).Bold();
        protected static readonly TextStyle LabelStyle = TextStyle.Default.FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
        protected static readonly TextStyle ValueStyle = TextStyle.Default.FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
        protected static readonly TextStyle HeaderLabelStyle = TextStyle.Default.FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2).Bold();
        protected static readonly TextStyle SmallValueStyle = TextStyle.Default.FontFamily(Fonts.Arial).FontSize(6).FontColor(Colors.Grey.Darken2);
        protected static readonly TextStyle SealStyle = TextStyle.Default.FontFamily(Fonts.Arial).FontSize(4).FontColor(Colors.Grey.Darken2);

        public Comprobante Comprobante { get; }

        protected Image? LogoImage { get; }

        protected CfdiDocumentBase(Comprobante comprobante, string? logoImagePath = null)
        {
            Comprobante = comprobante;

            if (!string.IsNullOrWhiteSpace(logoImagePath))
                LogoImage = Image.FromFile(logoImagePath);
        }

        /// <summary>Título mostrado en el recuadro del encabezado, p. ej. "CFDI de Nómina".</summary>
        protected abstract string DocumentTitle { get; }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(1.5F, Unit.Centimetre);

                // El encabezado y el bloque del timbre fluyen con el contenido para que
                // aparezcan solo en la primera y en la última página respectivamente.
                page.Content().Column(column =>
                {
                    column.Item().Element(ComposeHeader);
                    column.Item().Element(ComposeContent);
                    column.Item().Element(ComposeFooter);
                });

                page.Footer().Row(row =>
                {
                    row.RelativeItem().Text("Este documento es una representación impresa de un CFDI").Style(SmallValueStyle);

                    row.AutoItem().Text(text =>
                    {
                        text.DefaultTextStyle(SmallValueStyle);
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            });
        }

        protected abstract void ComposeContent(IContainer container);

        protected virtual void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.Spacing(10);

                if (LogoImage != null)
                    row.RelativeItem().AlignLeft().AlignMiddle().MaxHeight(50).Image(LogoImage).FitArea();
                else
                    row.RelativeItem();

                row.RelativeItem().Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                {
                    column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                        .Text(DocumentTitle).Style(TitleStyle).AlignCenter();

                    column.Item().Background(Colors.Grey.Lighten3).PaddingHorizontal(5).PaddingVertical(3).Row(row =>
                    {
                        row.Spacing(10);
                        row.RelativeItem(15).Text("Serie").Style(HeaderLabelStyle);
                        row.RelativeItem(20).Text("Folio").Style(HeaderLabelStyle);
                        row.RelativeItem(25).Text("Lugar").Style(HeaderLabelStyle);
                        row.RelativeItem(40).Text("Fecha y Hora").Style(HeaderLabelStyle).AlignEnd();
                    });

                    column.Item().PaddingHorizontal(5).PaddingVertical(2).Row(row =>
                    {
                        row.Spacing(10);
                        row.RelativeItem(15).Text(Comprobante.Serie).Style(ValueStyle);
                        row.RelativeItem(20).Text(Comprobante.Folio).Style(ValueStyle);
                        row.RelativeItem(25).Text(Comprobante.LugarExpedicion.ToString("00000")).Style(ValueStyle);
                        row.RelativeItem(40).Text(Comprobante.Fecha.ToString("yyyy-MM-ddTHH:mm:ss")).Style(ValueStyle).AlignEnd();
                    });
                });
            });
        }

        /// <summary>Bloque del Timbre Fiscal Digital; se coloca al final del contenido (última página).</summary>
        protected virtual void ComposeFooter(IContainer container)
        {
            container.PaddingTop(10).ShowEntire().Border(0.75F, Colors.Grey.Lighten3).CornerRadius(3).Padding(5).Row(row =>
                {
                    row.Spacing(5);

                    row.RelativeItem(20).Column(column =>
                    {
                        column.Item().Image(QrCodeGenerator(BuildVerificationUrl()));
                    });

                    row.RelativeItem(80).Column(column =>
                    {
                        column.Item().Background(Colors.Grey.Lighten3).CornerRadius(3).PaddingHorizontal(5).Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Text("Folio Fiscal:").Style(LabelStyle);
                            row.RelativeItem().Text("Número de Certificado SAT").Style(LabelStyle);
                            row.RelativeItem().Text("Fecha y Hora de Certificación").Style(LabelStyle);
                        });

                        column.Item().PaddingHorizontal(5).PaddingBottom(10).Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Text(Comprobante.TimbreFiscalDigital?.UUID).Style(SmallValueStyle);
                            row.RelativeItem().Text(Comprobante.TimbreFiscalDigital?.NoCertificadoSAT).Style(SmallValueStyle);
                            row.RelativeItem().Text(Comprobante.TimbreFiscalDigital?.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss")).Style(SmallValueStyle);
                        });

                        column.Item().Background(Colors.Grey.Lighten3).CornerRadius(3).PaddingHorizontal(5).Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Text("RFC Proveedor de Certificación").Style(LabelStyle);
                            row.RelativeItem().Text("Sello Digital del SAT").Style(LabelStyle);
                            row.RelativeItem().Text("Sello Digital del CFDI").Style(LabelStyle);
                        });

                        column.Item().PaddingHorizontal(5).PaddingBottom(10).Row(row =>
                        {
                            row.Spacing(10);
                            row.RelativeItem().Text(Comprobante.TimbreFiscalDigital?.RfcProvCertif).Style(SmallValueStyle);
                            row.RelativeItem().Text(Comprobante.TimbreFiscalDigital?.SelloSAT).Style(SealStyle).Justify();
                            row.RelativeItem().Text(Comprobante.TimbreFiscalDigital?.SelloCFD).Style(SealStyle).Justify();
                        });

                        column.Item().Background(Colors.Grey.Lighten3).CornerRadius(3).PaddingHorizontal(5).Row(row =>
                        {
                            row.RelativeItem().Text("Cadena Original del Timbre").Style(LabelStyle);
                        });

                        column.Item().PaddingHorizontal(5).Row(row =>
                        {
                            string cadenaTimbre = $"||{Comprobante.TimbreFiscalDigital?.Version}|{Comprobante.TimbreFiscalDigital?.UUID}|{Comprobante.TimbreFiscalDigital?.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss")}|" +
                                $"{Comprobante.TimbreFiscalDigital?.RfcProvCertif}|{Comprobante.TimbreFiscalDigital?.SelloCFD}|{Comprobante.TimbreFiscalDigital?.NoCertificadoSAT}||";

                            row.RelativeItem().Text(cadenaTimbre).Style(SealStyle).Justify();
                        });
                    });
                });
        }

        protected static string FormatMoney(decimal amount) => amount.ToString("C", MxCulture);

        private string BuildVerificationUrl()
        {
            // El parámetro "fe" corresponde a los últimos 8 caracteres del sello digital del CFDI.
            string? sello = Comprobante.TimbreFiscalDigital?.SelloCFD;
            string fe = sello?.Length >= 8 ? sello[^8..] : sello ?? string.Empty;

            return $"https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?id={Comprobante.TimbreFiscalDigital?.UUID}&re={Comprobante.Emisor.Rfc}" +
                $"&rr={Comprobante.Receptor.Rfc}&tt={Comprobante.Total.ToString(CultureInfo.InvariantCulture)}&fe={Uri.EscapeDataString(fe)}";
        }

        private static byte[] QrCodeGenerator(string text)
        {
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            using QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20, false);
        }
    }
}
