namespace Core.Pdf.Documents;

using Core.Abstractions;
using Core.Catalogs;
using Core.Common;
using Core.Formatting;
using Core.Models.Base;
using Core.Models.Complements.Pagos20;
using Core.Pdf.Components;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Representación impresa de CFDI de Recepción de Pagos 2.0 (Tipo P).
/// </summary>
public class RecepcionPagosDocument : CfdiDocumentBase
{
    private readonly IEnumerable<ICfdiComplementSectionRenderer> _complementRenderers;

    public RecepcionPagosDocument(
        Comprobante comprobante,
        CfdiConversionOptions options,
        IEnumerable<ICfdiComplementSectionRenderer>? complementRenderers = null)
        : base(comprobante, options)
    {
        _complementRenderers = (complementRenderers ?? []).OrderBy(r => r.Order);
    }

    protected override string DocumentTitle => "CFDI de Recepción de Pagos";

    protected override void ComposeContent(IContainer container)
    {
        container.PaddingVertical(6).Column(column =>
        {
            column.Spacing(6);

            // 1. Emisor y Receptor
            column.Item().Component(new IssuerRecipientComponent(Comprobante));

            // 2. CFDI Relacionados (si existen)
            if (Comprobante.CfdiRelacionados.Length > 0)
            {
                column.Item().Component(new RelatedCfdiComponent(Comprobante.CfdiRelacionados));
            }

            Pagos? complementoPagos = Comprobante.Pagos;
            if (complementoPagos != null)
            {
                // 3. Totales de Pagos 2.0 (si están definidos)
                if (complementoPagos.Totales != null)
                {
                    PagosTotales totales = complementoPagos.Totales;
                    column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                    {
                        col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                            .Text("Resumen de Totales del Complemento de Pagos").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                        col.Item().Padding(4).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t => { t.Span("Monto Total de Pagos: ").Style(CfdiPdfTheme.LabelStyle); t.Span(totales.MontoTotalPagos.ToString("C")).Style(CfdiPdfTheme.ValueBoldStyle); });
                                if (totales.TotalTrasladosBaseIVA16Specified)
                                    c.Item().Text(t => { t.Span("Base IVA 16%: ").Style(CfdiPdfTheme.LabelStyle); t.Span(totales.TotalTrasladosBaseIVA16.ToString("C")).Style(CfdiPdfTheme.ValueStyle); });
                                if (totales.TotalTrasladosImpuestoIVA16Specified)
                                    c.Item().Text(t => { t.Span("Impuesto IVA 16%: ").Style(CfdiPdfTheme.LabelStyle); t.Span(totales.TotalTrasladosImpuestoIVA16.ToString("C")).Style(CfdiPdfTheme.ValueStyle); });
                            });

                            row.RelativeItem().Column(c =>
                            {
                                if (totales.TotalRetencionesIVASpecified)
                                    c.Item().Text(t => { t.Span("Retención IVA: ").Style(CfdiPdfTheme.LabelStyle); t.Span(totales.TotalRetencionesIVA.ToString("C")).Style(CfdiPdfTheme.ValueStyle); });
                                if (totales.TotalRetencionesISRSpecified)
                                    c.Item().Text(t => { t.Span("Retención ISR: ").Style(CfdiPdfTheme.LabelStyle); t.Span(totales.TotalRetencionesISR.ToString("C")).Style(CfdiPdfTheme.ValueStyle); });
                                if (totales.TotalRetencionesIEPSSpecified)
                                    c.Item().Text(t => { t.Span("Retención IEPS: ").Style(CfdiPdfTheme.LabelStyle); t.Span(totales.TotalRetencionesIEPS.ToString("C")).Style(CfdiPdfTheme.ValueStyle); });
                            });
                        });
                    });
                }

                // 4. Detalle de Pagos y Documentos Relacionados
                int indexPago = 1;
                foreach (PagosPago pago in complementoPagos.Pago)
                {
                    column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                    {
                        col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                            .Text($"Información del Pago #{indexPago}").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                        // Encabezado del pago
                        col.Item().Padding(3).Row(pagoRow =>
                        {
                            pagoRow.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t => { t.Span("Fecha de Pago: ").Style(CfdiPdfTheme.LabelStyle); t.Span(pago.FechaPago.ToString("yyyy-MM-dd HH:mm:ss")).Style(CfdiPdfTheme.ValueStyle); });
                                c.Item().Text(t => { t.Span("Forma de Pago: ").Style(CfdiPdfTheme.LabelStyle); t.Span(SatCatalogProvider.GetFormaPago(pago.FormaDePagoP)).Style(CfdiPdfTheme.ValueStyle); });
                                if (!string.IsNullOrWhiteSpace(pago.NumOperacion))
                                    c.Item().Text(t => { t.Span("No. Operación: ").Style(CfdiPdfTheme.LabelStyle); t.Span(pago.NumOperacion).Style(CfdiPdfTheme.ValueStyle); });
                            });

                            pagoRow.RelativeItem().Column(c =>
                            {
                                c.Item().Text(t => { t.Span("Moneda de Pago: ").Style(CfdiPdfTheme.LabelStyle); t.Span(pago.MonedaP).Style(CfdiPdfTheme.ValueStyle); });
                                if (pago.TipoCambioPSpecified && pago.TipoCambioP > 0)
                                    c.Item().Text(t => { t.Span("Tipo de Cambio: ").Style(CfdiPdfTheme.LabelStyle); t.Span(pago.TipoCambioP.ToString("N4")).Style(CfdiPdfTheme.ValueStyle); });
                                c.Item().Text(t => { t.Span("Monto Pagado: ").Style(CfdiPdfTheme.LabelStyle); t.Span(pago.Monto.ToString("C")).Style(CfdiPdfTheme.ValueBoldStyle); });
                            });
                        });

                        // Cuentas bancarias si existen
                        if (!string.IsNullOrWhiteSpace(pago.CtaOrdenante) || !string.IsNullOrWhiteSpace(pago.CtaBeneficiario))
                        {
                            col.Item().PaddingHorizontal(3).PaddingBottom(2).Row(ctaRow =>
                            {
                                if (!string.IsNullOrWhiteSpace(pago.CtaOrdenante))
                                    ctaRow.RelativeItem().Text($"Cta. Ordenante: {pago.CtaOrdenante} ({pago.RfcEmisorCtaOrd ?? "N/A"})").Style(CfdiPdfTheme.SmallValueStyle);
                                if (!string.IsNullOrWhiteSpace(pago.CtaBeneficiario))
                                    ctaRow.RelativeItem().Text($"Cta. Beneficiario: {pago.CtaBeneficiario} ({pago.RfcEmisorCtaBen ?? "N/A"})").Style(CfdiPdfTheme.SmallValueStyle);
                            });
                        }

                        // Tabla de Documentos Relacionados
                        if (pago.DoctoRelacionado.Count > 0)
                        {
                            col.Item().Table(drTable =>
                            {
                                drTable.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3.5F); // Folio Fiscal (UUID)
                                    columns.RelativeColumn(1.0F); // Serie/Folio
                                    columns.RelativeColumn(0.8F); // Moneda
                                    columns.RelativeColumn(0.8F); // Parcialidad
                                    columns.RelativeColumn(1.3F); // Saldo Ant.
                                    columns.RelativeColumn(1.3F); // Pagado
                                    columns.RelativeColumn(1.3F); // Insoluto
                                });

                                drTable.Header(drHeader =>
                                {
                                    drHeader.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("UUID Relacionado").Style(CfdiPdfTheme.LabelStyle);
                                    drHeader.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Serie/Folio").Style(CfdiPdfTheme.LabelStyle);
                                    drHeader.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Moneda").Style(CfdiPdfTheme.LabelStyle);
                                    drHeader.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Parc.").Style(CfdiPdfTheme.LabelStyle).AlignCenter();
                                    drHeader.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Saldo Anterior").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                                    drHeader.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Importe Pagado").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                                    drHeader.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Saldo Insoluto").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                                });

                                foreach (PagosPagoDoctoRelacionado dr in pago.DoctoRelacionado)
                                {
                                    drTable.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dr.IdDocumento).Style(CfdiPdfTheme.SmallValueStyle);
                                    string serieFolio = $"{dr.Serie}{dr.Folio}";
                                    drTable.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(string.IsNullOrWhiteSpace(serieFolio) ? "-" : serieFolio).Style(CfdiPdfTheme.ValueStyle);
                                    drTable.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dr.MonedaDR).Style(CfdiPdfTheme.ValueStyle);
                                    drTable.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dr.NumParcialidad).Style(CfdiPdfTheme.ValueStyle).AlignCenter();
                                    drTable.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dr.ImpSaldoAnt.ToString("C")).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                                    drTable.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dr.ImpPagado.ToString("C")).Style(CfdiPdfTheme.ValueBoldStyle).AlignRight();
                                    drTable.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dr.ImpSaldoInsoluto.ToString("C")).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                                }
                            });
                        }
                    });
                    indexPago++;
                }
            }

            // 5. Complementos Modulares Inyectados
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
