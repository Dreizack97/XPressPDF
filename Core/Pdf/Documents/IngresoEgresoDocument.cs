namespace Core.Pdf.Documents;

using Core.Abstractions;
using Core.Catalogs;
using Core.Common;
using Core.Formatting;
using Core.Models.Base;
using Core.Pdf.Components;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Representación impresa de CFDI de Ingreso (I) y Egreso / Nota de Crédito (E) conforme al Anexo 20 y RMF 2.7.1.7.
/// </summary>
public class IngresoEgresoDocument : CfdiDocumentBase
{
    private readonly IEnumerable<ICfdiComplementSectionRenderer> _complementRenderers;

    public IngresoEgresoDocument(
        Comprobante comprobante,
        CfdiConversionOptions options,
        IEnumerable<ICfdiComplementSectionRenderer>? complementRenderers = null)
        : base(comprobante, options)
    {
        _complementRenderers = (complementRenderers ?? []).OrderBy(r => r.Order);
    }

    protected override string DocumentTitle =>
        $"CFDI de {SatCatalogProvider.GetTipoComprobanteName(Comprobante.TipoDeComprobante)}";

    protected override void ComposeContent(IContainer container)
    {
        container.PaddingVertical(6).Column(column =>
        {
            column.Spacing(6);

            // 1. Datos de Emisor y Receptor (Tarjetas simétricas con ExtendVertical)
            column.Item().Component(new IssuerRecipientComponent(Comprobante));

            // 2. Información Global (si aplica)
            if (Comprobante.InformacionGlobal is not null)
            {
                column.Item().Component(new GlobalInfoComponent(Comprobante.InformacionGlobal));
            }

            // 3. CFDI Relacionados (crucial para Notas de Crédito / Egreso y sustituciones)
            if (Comprobante.CfdiRelacionados is { Length: > 0 })
            {
                column.Item().Component(new RelatedCfdiComponent(Comprobante.CfdiRelacionados));
            }

            // 4. Datos Generales de la Operación
            column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
            {
                col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                    .Text("Datos Generales del Comprobante").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1.3F);
                        columns.RelativeColumn(1.3F);
                        columns.RelativeColumn(0.7F);
                        columns.RelativeColumn(0.7F);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Forma de Pago").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Método de Pago").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Moneda").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Exportación").Style(CfdiPdfTheme.LabelStyle);
                    });

                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(SatCatalogProvider.GetFormaPago(Comprobante.FormaPago)).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(SatCatalogProvider.GetMetodoPago(Comprobante.MetodoPago)).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(Comprobante.Moneda).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(SatCatalogProvider.GetExportacion(Comprobante.Exportacion)).Style(CfdiPdfTheme.ValueStyle);
                });

                // Si hay condiciones de pago o tipo de cambio, mostrarlos en una fila secundaria
                if (!string.IsNullOrWhiteSpace(Comprobante.CondicionesDePago) || (Comprobante.TipoCambioSpecified && Comprobante.TipoCambio > 0))
                {
                    col.Item().PaddingHorizontal(6).PaddingVertical(3).Row(r =>
                    {
                        r.Spacing(15);
                        if (!string.IsNullOrWhiteSpace(Comprobante.CondicionesDePago))
                        {
                            r.AutoItem().Text(t => CfdiPdfTheme.KeyValue(t, "Condiciones de Pago:", Comprobante.CondicionesDePago));
                        }

                        if (Comprobante.TipoCambioSpecified && Comprobante.TipoCambio > 0)
                        {
                            r.AutoItem().Text(t => CfdiPdfTheme.KeyValue(t, "Tipo de Cambio:", Comprobante.TipoCambio.ToString("N4")));
                        }
                    });
                }
            });

            // 5. Tabla de Conceptos
            column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
            {
                col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                    .Text("Conceptos").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                bool hasDiscount = Comprobante.Conceptos.Any(c => c.DescuentoSpecified && c.Descuento > 0);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);  // Clave SAT
                        columns.ConstantColumn(40);  // Cantidad
                        columns.ConstantColumn(35);  // Clave Unidad
                        columns.RelativeColumn(0.9F);// Unidad
                        columns.RelativeColumn(3.2F);// Descripción
                        columns.RelativeColumn(1.0F);// Valor Unitario
                        if (hasDiscount)
                            columns.RelativeColumn(0.9F); // Descuento
                        columns.ConstantColumn(44);  // Objeto Imp (44pt para evitar salto de línea en Obj.Imp)
                        columns.RelativeColumn(1.0F);// Importe
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Clave SAT").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Cant.").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Clave").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Unidad").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Descripción").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Valor Unit.").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        if (hasDiscount)
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Descuento").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Obj.Imp").Style(CfdiPdfTheme.LabelStyle).AlignCenter();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Importe").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                    });

                    foreach (ComprobanteConcepto concepto in Comprobante.Conceptos)
                    {
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.ClaveProdServ).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.Cantidad.ToString("0.##")).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.ClaveUnidad).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.Unidad).Style(CfdiPdfTheme.ValueStyle);

                        // Descripción con soporte para número de identificación, pedimento, cuenta predial, terceros e IEDU
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Column(descCol =>
                        {
                            descCol.Item().Text(concepto.Descripcion).Style(CfdiPdfTheme.ValueStyle);

                            if (!string.IsNullOrWhiteSpace(concepto.NoIdentificacion))
                            {
                                descCol.Item().Text($"No. Identificación: {concepto.NoIdentificacion}").Style(CfdiPdfTheme.SmallValueStyle);
                            }

                            if (concepto.InformacionAduanera != null)
                            {
                                foreach (var aduana in concepto.InformacionAduanera)
                                {
                                    descCol.Item().Text($"Pedimento Aduanal: {aduana.NumeroPedimento}").Style(CfdiPdfTheme.SmallValueStyle);
                                }
                            }

                            if (concepto.CuentaPredial != null)
                            {
                                foreach (var predial in concepto.CuentaPredial)
                                {
                                    descCol.Item().Text($"Cuenta Predial: {predial.Numero}").Style(CfdiPdfTheme.SmallValueStyle);
                                }
                            }

                            if (concepto.ACuentaTerceros != null)
                            {
                                descCol.Item().Text($"A Cuenta de Terceros: {concepto.ACuentaTerceros.RfcACuentaTerceros} ({concepto.ACuentaTerceros.NombreACuentaTerceros})").Style(CfdiPdfTheme.SmallValueStyle);
                            }

                            if (concepto.InstitucionesEducativas is not null)
                            {
                                var iedu = concepto.InstitucionesEducativas;
                                descCol.Item().Text($"[IEDU] Alumno: {iedu.nombreAlumno} | CURP: {iedu.CURP} | Nivel: {iedu.nivelEducativo} | RVOE: {iedu.autRVOE}").Style(CfdiPdfTheme.SmallValueStyle);
                            }
                        });

                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(concepto.ValorUnitario)).Style(CfdiPdfTheme.ValueStyle).AlignRight();

                        if (hasDiscount)
                        {
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle)
                                .Text(concepto.DescuentoSpecified ? FormatMoney(concepto.Descuento) : string.Empty)
                                .Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        }

                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(concepto.ObjetoImp).Style(CfdiPdfTheme.ValueStyle).AlignCenter();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(concepto.Importe)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    }
                });
            });

            // 6. Totales e Importe con Letra (Simetría de altura mediante ExtendVertical)
            column.Item().Row(row =>
            {
                row.Spacing(10);

                // Importe con Letra
                row.RelativeItem(60).Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Importe con Letra").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Padding(6).Text(CurrencyToWordsConverter.ToText(Comprobante.Total, Comprobante.Moneda))
                        .Style(CfdiPdfTheme.ValueStyle);
                });

                // Desglose de Totales e Impuestos
                row.RelativeItem(40).Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    TotalItem(col, "Subtotal:", FormatMoney(Comprobante.SubTotal));

                    if (Comprobante.DescuentoSpecified && Comprobante.Descuento > 0)
                    {
                        TotalItem(col, "Descuento:", FormatMoney(Comprobante.Descuento));
                    }

                    foreach (ComprobanteImpuestosTraslado traslado in Comprobante.Impuestos?.Traslados ?? [])
                    {
                        string label = traslado.TipoFactor == "Exento"
                            ? $"{SatCatalogProvider.GetImpuesto(traslado.Impuesto)} Exento:"
                            : $"{SatCatalogProvider.GetImpuesto(traslado.Impuesto)} {traslado.TasaOCuota * 100:0.##}%:";

                        TotalItem(col, label, traslado.ImporteSpecified ? FormatMoney(traslado.Importe) : "Exento");
                    }

                    foreach (ComprobanteImpuestosRetencion retencion in Comprobante.Impuestos?.Retenciones ?? [])
                    {
                        TotalItem(col, $"Retención {SatCatalogProvider.GetImpuesto(retencion.Impuesto)}:", FormatMoney(retencion.Importe));
                    }

                    col.Item().Background(CfdiPdfTheme.LightBackground).PaddingHorizontal(5).PaddingVertical(3).Row(r =>
                    {
                        r.RelativeItem().Text("Total:").Style(CfdiPdfTheme.LabelStyle);
                        r.RelativeItem().Text($"{FormatMoney(Comprobante.Total)} {Comprobante.Moneda}").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                    });
                });
            });

            // 7. Secciones de Complementos Registrados (Extensibilidad OCP)
            foreach (ICfdiComplementSectionRenderer renderer in _complementRenderers)
            {
                if (renderer.CanRender(Comprobante))
                {
                    renderer.Render(column, Comprobante);
                }
            }
        });
    }

    private static void TotalItem(ColumnDescriptor column, string label, string value)
    {
        column.Item().BorderBottom(0.5F).BorderColor(CfdiPdfTheme.BorderColor).PaddingHorizontal(5).PaddingVertical(1.5F).Row(row =>
        {
            row.RelativeItem().Text(label).Style(CfdiPdfTheme.LabelStyle);
            row.RelativeItem().Text(value).Style(CfdiPdfTheme.ValueStyle).AlignRight();
        });
    }
}
