using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Schemas.Base;
using Schemas.Complements.ValesDeDespensa;

namespace BLL.Utilities
{
    /// <summary>
    /// Representación impresa de un CFDI con complemento de Vales de Despensa: reutiliza el diseño
    /// de la factura comercial y agrega el detalle de la dispersión por beneficiario.
    /// </summary>
    public class ValesDespensaDocument : InvoiceDocument
    {
        public ValesDespensaDocument(Comprobante comprobante, string? logoImagePath = null)
            : base(comprobante, logoImagePath)
        {
        }

        protected override string DocumentTitle => "CFDI de Vales de Despensa";

        protected override void ComposeExtraSections(ColumnDescriptor column)
        {
            ValesDeDespensa? vales = Comprobante.ValesDespensa;

            if (vales == null)
                return;

            column.Item().Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
            {
                column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                    .Text("Complemento de Vales de Despensa").Style(TitleStyle).AlignCenter();

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1.5F);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCellStyle).Text("Tipo de Operación").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("Registro Patronal").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("Número de Cuenta").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("Total Dispersado").Style(LabelStyle).AlignRight();
                    });

                    table.Cell().Element(BodyCellStyle).Text(vales.TipoOperacion).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(vales.RegistroPatronal).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(vales.NumeroDeCuenta).Style(ValueStyle);
                    table.Cell().Element(BodyCellStyle).Text(FormatMoney(vales.Total)).Style(ValueStyle).AlignRight();
                });
            });

            ValesDeDespensaConceptosConcepto[] conceptos = vales.Conceptos?.Concepto ?? Array.Empty<ValesDeDespensaConceptosConcepto>();

            if (conceptos.Length == 0)
                return;

            column.Item().Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
            {
                column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                    .Text($"Dispersión de Vales de Despensa ({conceptos.Length} beneficiarios)").Style(TitleStyle).AlignCenter();

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(55);  // Identificador
                        columns.ConstantColumn(60);  // Fecha
                        columns.ConstantColumn(80);  // RFC
                        columns.ConstantColumn(100); // CURP
                        columns.RelativeColumn(2);   // Nombre
                        columns.RelativeColumn(1);   // NSS
                        columns.RelativeColumn(1);   // Importe
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCellStyle).Text("Identificador").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("Fecha").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("RFC").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("CURP").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("Nombre").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("NSS").Style(LabelStyle);
                        header.Cell().Element(HeaderCellStyle).Text("Importe").Style(LabelStyle).AlignRight();
                    });

                    foreach (ValesDeDespensaConceptosConcepto concepto in conceptos)
                    {
                        table.Cell().Element(BodyCellStyle).Text(concepto.identificador).Style(ValueStyle);
                        table.Cell().Element(BodyCellStyle).Text(concepto.fecha.ToString("yyyy-MM-dd")).Style(ValueStyle);
                        table.Cell().Element(BodyCellStyle).Text(concepto.rfc).Style(ValueStyle);
                        table.Cell().Element(BodyCellStyle).Text(concepto.curp).Style(ValueStyle);
                        table.Cell().Element(BodyCellStyle).Text(concepto.nombre?.Trim()).Style(ValueStyle);
                        table.Cell().Element(BodyCellStyle).Text(concepto.numSeguridad).Style(ValueStyle);
                        table.Cell().Element(BodyCellStyle).Text(FormatMoney(concepto.importe)).Style(ValueStyle).AlignRight();
                    }

                    table.Footer(footer =>
                    {
                        footer.Cell().ColumnSpan(6).Element(HeaderCellStyle).Text("Total").Style(LabelStyle).AlignRight();
                        footer.Cell().Element(HeaderCellStyle).Text(FormatMoney(vales.Total)).Style(LabelStyle).AlignRight();
                    });
                });
            });
        }
    }
}
