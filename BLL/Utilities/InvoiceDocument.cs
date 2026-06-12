using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Schemas.Base;

namespace BLL.Utilities
{
    /// <summary>
    /// Representación impresa de un CFDI de Ingreso/Egreso (factura comercial) conforme a los
    /// requisitos del Anexo 20 y la regla 2.7.1.7 de la RMF: datos de emisor y receptor, conceptos,
    /// impuestos, totales e importe con letra.
    /// </summary>
    public class InvoiceDocument : CfdiDocumentBase
    {
        public InvoiceDocument(Comprobante comprobante, string? logoImagePath = null)
            : base(comprobante, logoImagePath)
        {
        }

        protected override string DocumentTitle => $"CFDI de {CfdiCatalogs.GetTipoComprobanteName(Comprobante.TipoDeComprobante)}";

        protected override void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                column.Spacing(10);

                // Emisor y Receptor
                column.Item().Row(row =>
                {
                    row.Spacing(10);

                    row.RelativeItem().Border(1, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                    {
                        column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                            .Text("Datos del Emisor").Style(TitleStyle).AlignCenter();

                        column.Item().PaddingHorizontal(3).PaddingBottom(3).Element(ComposeIssuer);
                    });

                    row.RelativeItem().Border(1, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                    {
                        column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                            .Text("Datos del Receptor").Style(TitleStyle).AlignCenter();

                        column.Item().PaddingHorizontal(3).PaddingBottom(3).Element(ComposeRecipient);
                    });
                });

                // Datos generales del comprobante
                column.Item().Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                {
                    column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                        .Text("Datos Generales del Comprobante").Style(TitleStyle).AlignCenter();

                    column.Item().Element(ComposeGeneralData);
                });

                // Conceptos
                column.Item().Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                {
                    column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                        .Text("Conceptos").Style(TitleStyle).AlignCenter();

                    column.Item().Element(ComposeConcepts);
                });

                // Totales e importe con letra
                column.Item().Element(ComposeTotals);

                ComposeExtraSections(column);
            });
        }

        /// <summary>Punto de extensión para que los comprobantes derivados agreguen secciones propias.</summary>
        protected virtual void ComposeExtraSections(ColumnDescriptor column)
        {
        }

        private void ComposeIssuer(IContainer container)
        {
            ComprobanteEmisor emisor = Comprobante.Emisor;

            container.ShowEntire().Column(column =>
            {
                column.Spacing(1);

                column.Item().Text(emisor.Nombre).Style(LabelStyle).ExtraBold();

                LabeledText(column, "RFC:", emisor.Rfc);
                LabeledText(column, "Régimen Fiscal:", CfdiCatalogs.GetRegimenFiscal(emisor.RegimenFiscal));
                LabeledText(column, "No. de Certificado:", Comprobante.NoCertificado);
            });
        }

        private void ComposeRecipient(IContainer container)
        {
            ComprobanteReceptor receptor = Comprobante.Receptor;

            container.ShowEntire().Column(column =>
            {
                column.Spacing(1);

                column.Item().Text(receptor.Nombre).Style(LabelStyle).ExtraBold();

                LabeledText(column, "RFC:", receptor.Rfc);
                LabeledText(column, "Régimen Fiscal:", CfdiCatalogs.GetRegimenFiscal(receptor.RegimenFiscalReceptor));
                LabeledText(column, "Domicilio Fiscal:", receptor.DomicilioFiscalReceptor);
                LabeledText(column, "Uso del CFDI:", CfdiCatalogs.GetUsoCfdi(receptor.UsoCFDI));
            });
        }

        private void ComposeGeneralData(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn(0.6F);
                    columns.RelativeColumn(0.6F);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCellStyle).Text("Forma de Pago").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Método de Pago").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Moneda").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Exportación").Style(LabelStyle);
                });

                table.Cell().Element(BodyCellStyle).Text(CfdiCatalogs.GetFormaPago(Comprobante.FormaPago)).Style(ValueStyle);
                table.Cell().Element(BodyCellStyle).Text(CfdiCatalogs.GetMetodoPago(Comprobante.MetodoPago)).Style(ValueStyle);
                table.Cell().Element(BodyCellStyle).Text(Comprobante.Moneda).Style(ValueStyle);
                table.Cell().Element(BodyCellStyle).Text(CfdiCatalogs.GetExportacion(Comprobante.Exportacion)).Style(ValueStyle);
            });
        }

        private void ComposeConcepts(IContainer container)
        {
            bool hasDiscount = Comprobante.Conceptos.Any(c => c.DescuentoSpecified);

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(55);  // Clave Prod/Serv
                    columns.ConstantColumn(45);  // Cantidad
                    columns.ConstantColumn(40);  // Clave Unidad
                    columns.RelativeColumn(1);   // Unidad
                    columns.RelativeColumn(3);   // Descripción
                    columns.RelativeColumn(1);   // Valor Unitario

                    if (hasDiscount)
                        columns.RelativeColumn(1); // Descuento

                    columns.RelativeColumn(1);   // Importe
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCellStyle).Text("Clave SAT").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Cantidad").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Clave Unidad").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Unidad").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Descripción").Style(LabelStyle);
                    header.Cell().Element(HeaderCellStyle).Text("Valor Unitario").Style(LabelStyle).AlignRight();

                    if (hasDiscount)
                        header.Cell().Element(HeaderCellStyle).Text("Descuento").Style(LabelStyle).AlignRight();

                    header.Cell().Element(HeaderCellStyle).Text("Importe").Style(LabelStyle).AlignRight();
                });

                foreach (ComprobanteConcepto concepto in Comprobante.Conceptos)
                {
                    table.Cell().Element(BodyCellStyle).Text(concepto.ClaveProdServ).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(concepto.Cantidad.ToString("0.##")).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(concepto.ClaveUnidad).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(concepto.Unidad).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(concepto.Descripcion).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(FormatMoney(concepto.ValorUnitario)).Style(ValueStyle).AlignRight();

                    if (hasDiscount)
                        table.Cell().Element(BodyCellStyle).Text(concepto.DescuentoSpecified ? FormatMoney(concepto.Descuento) : string.Empty).Style(ValueStyle).AlignRight();

                    table.Cell().Element(BodyCellStyle).Text(FormatMoney(concepto.Importe)).Style(ValueStyle).AlignRight();
                }
            });
        }

        private void ComposeTotals(IContainer container)
        {
            container.Row(row =>
            {
                row.Spacing(10);

                // Importe con letra
                row.RelativeItem(60).Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                {
                    column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                        .Text("Importe con Letra").Style(TitleStyle).AlignCenter();

                    column.Item().Padding(5).Text(CurrencyToText.ToText(Comprobante.Total, Comprobante.Moneda)).Style(ValueStyle);
                });

                // Totales
                row.RelativeItem(40).Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                {
                    TotalRow(column, "Subtotal:", FormatMoney(Comprobante.SubTotal));

                    if (Comprobante.DescuentoSpecified)
                        TotalRow(column, "Descuento:", FormatMoney(Comprobante.Descuento));

                    foreach (ComprobanteImpuestosTraslado traslado in Comprobante.Impuestos?.Traslados ?? Array.Empty<ComprobanteImpuestosTraslado>())
                    {
                        string label = traslado.TipoFactor == "Exento"
                            ? $"{CfdiCatalogs.GetImpuesto(traslado.Impuesto)} Exento:"
                            : $"{CfdiCatalogs.GetImpuesto(traslado.Impuesto)} {traslado.TasaOCuota * 100:0.##} %:";

                        TotalRow(column, label, traslado.ImporteSpecified ? FormatMoney(traslado.Importe) : string.Empty);
                    }

                    foreach (ComprobanteImpuestosRetencion retencion in Comprobante.Impuestos?.Retenciones ?? Array.Empty<ComprobanteImpuestosRetencion>())
                        TotalRow(column, $"Ret. {CfdiCatalogs.GetImpuesto(retencion.Impuesto)}:", FormatMoney(retencion.Importe));

                    column.Item().Background(Colors.Grey.Lighten3).PaddingHorizontal(5).PaddingVertical(3).Row(row =>
                    {
                        row.RelativeItem().Text("Total:").Style(LabelStyle);
                        row.RelativeItem().Text($"{FormatMoney(Comprobante.Total)} {Comprobante.Moneda}").Style(LabelStyle).AlignRight();
                    });
                });
            });
        }

        protected void LabeledText(ColumnDescriptor column, string label, string? value)
        {
            column.Item().Row(row =>
            {
                row.Spacing(2);
                row.AutoItem().Text(label).Style(LabelStyle);
                row.AutoItem().Text(value).Style(ValueStyle);
            });
        }

        private void TotalRow(ColumnDescriptor column, string label, string value)
        {
            column.Item().BorderBottom(0.75F).BorderColor(Colors.Grey.Lighten2).PaddingHorizontal(5).PaddingVertical(2).Row(row =>
            {
                row.RelativeItem().Text(label).Style(LabelStyle);
                row.RelativeItem().Text(value).Style(ValueStyle).AlignRight();
            });
        }

        protected static IContainer HeaderCellStyle(IContainer container) =>
            container.Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingHorizontal(5);

        protected static IContainer BodyCellStyle(IContainer container) =>
            container.BorderBottom(0.75F).BorderColor(Colors.Grey.Lighten2).PaddingVertical(1.5F).PaddingHorizontal(5);
    }
}
