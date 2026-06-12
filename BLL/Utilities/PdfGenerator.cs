using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Schemas.Base;
using Schemas.Complements.Nomina;

namespace BLL.Utilities
{
    public class PdfGenerator : CfdiDocumentBase
    {
        public PdfGenerator(Comprobante comprobante, string? logoImagePath = null)
            : base(comprobante, logoImagePath)
        {
        }

        protected override string DocumentTitle => "CFDI de Nómina";

        protected override void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(column =>
            {
                column.Spacing(10);

                // General Data
                column.Item().Row(row =>
                {
                    row.Spacing(10);

                    row.RelativeItem().Border(1, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                    {
                        column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                            .Text("Datos del Emisor").FontFamily(Fonts.Arial).FontSize(10).FontColor(Colors.Grey.Darken3).Bold().AlignCenter();

                        column.Item().PaddingHorizontal(3).Row(row =>
                        {
                            row.RelativeItem().Component(new CompanyComponent(Comprobante.Emisor, Comprobante?.Nomina?.Emisor!));
                        });
                    });

                    row.RelativeItem().Border(1, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                    {
                        column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                            .Text("Datos del Receptor").FontFamily(Fonts.Arial).FontSize(10).FontColor(Colors.Grey.Darken3).Bold().AlignCenter();

                        column.Item().PaddingHorizontal(3).PaddingBottom(3).Row(row =>
                        {
                            row.RelativeItem().Component(new EmployeeComponent(Comprobante.Receptor, Comprobante.Nomina.Receptor));
                        });
                    });
                });

                // Perceptions & Deductions
                column.Item().Row(row =>
                {
                    row.Spacing(10);

                    row.RelativeItem().Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                    {
                        column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                            .Text("Percepciones").FontFamily(Fonts.Arial).FontSize(10).FontColor(Colors.Grey.Darken3).Bold().AlignCenter();

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Component(new PerceptionsComponent(Comprobante.Nomina.Percepciones));
                        });
                    });

                    row.RelativeItem().Border(0.75F, Colors.Grey.Lighten2).CornerRadius(3).Column(column =>
                    {
                        column.Item().Background(Colors.Grey.Lighten2).Padding(3)
                            .Text("Deducciones").FontFamily(Fonts.Arial).FontSize(10).FontColor(Colors.Grey.Darken3).Bold().AlignCenter();

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Component(new DeductionsComponent(Comprobante.Descuento.ToString("C"), Comprobante.Nomina.Deducciones));
                        });
                    });
                });

                // Sumary Payroll
                column.Item().Row(row =>
                {
                    row.RelativeItem().Border(0.75F, Colors.Grey.Lighten3).CornerRadius(3).Column(column =>
                    {
                        column.Item().Element(SummaryPayrroll);
                    });
                });

                // Signature Payrroll
                column.Item().PaddingTop(20).Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Element(SignaturePayrroll);
                    });
                });
            });
        }

        private void SummaryPayrroll(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn(1.5F);
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Fecha Inicial").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Fecha Final").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Fecha de Pago").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Días Pagados").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Salario Base").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Salario Integrado").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Subtotal").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Descuento").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Total").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();

                    static IContainer CellStyle(IContainer container) => container.Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingLeft(5).AlignCenter().AlignMiddle();
                });

                table.Cell().Element(CellStyle).Text(Comprobante.Nomina.FechaInicialPago.ToString("yyyy-MM-dd")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text(Comprobante.Nomina.FechaFinalPago.ToString("yyyy-MM-dd")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text(Comprobante.Nomina.FechaPago.ToString("yyyy-MM-dd")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text(Comprobante.Nomina.NumDiasPagados.ToString()).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text(Comprobante.Nomina.Receptor.SalarioBaseCotApor.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text(Comprobante.Nomina.Receptor.SalarioDiarioIntegrado.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text(Comprobante.SubTotal.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text(Comprobante.Descuento.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                table.Cell().Element(CellStyle).Text($"{Comprobante.Total:C} MXN").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2).Bold();

                static IContainer CellStyle(IContainer container) => container.BorderBottom(0.75F).BorderColor(Colors.Grey.Lighten2).PaddingVertical(1.5F).PaddingLeft(5).AlignCenter();
            });
        }

        private void SignaturePayrroll(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(70);
                    columns.RelativeColumn(30);
                });

                string description = $"RECIBÍ DE LA EMPRESA {Comprobante.Emisor.Nombre} LA CANTIDAD DE {Comprobante.Total.ToString("C")} MXN MISMA QUE CUBREN LAS PERCEPCIONES QUE ME " +
                    $"CORRESPONDEN EN EL PERIODO INDICADO, NO EXISTIENDO NINGÚN ADEUDO POR PARTE DE LA EMPRESA PARA EL SUSCRITO, PUES ESTOY TOTALMENTE PAGADO DE MIS SALARIOS Y " +
                    $"PRESTACIONES DEVENGADAS HASTA LA FECHA.";

                table.Cell().Text(description).FontFamily(Fonts.Arial).FontSize(6).FontColor(Colors.Grey.Darken2).Justify();
                table.Cell();
                table.Cell();
                table.Cell().Element(CellStyle).Text("Firma del Empleado").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);

                static IContainer CellStyle(IContainer container) => container.BorderTop(1).BorderColor(Colors.Grey.Darken2).AlignCenter();
            });
        }

    }

    public class CompanyComponent : IComponent
    {
        private ComprobanteEmisor Emisor { get; }

        private NominaEmisor Nomina { get; }

        public CompanyComponent(ComprobanteEmisor emisor, NominaEmisor nomina)
        {
            Emisor = emisor;
            Nomina = nomina;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(1);

                column.Item().Text(Emisor.Nombre).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).ExtraBold();

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("RFC:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Emisor.Rfc).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("Régimen Fiscal:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Emisor.RegimenFiscal.ToString()).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("Registro Patronal:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.RegistroPatronal.ToString()).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });
            });
        }
    }

    public class EmployeeComponent : IComponent
    {
        private ComprobanteReceptor Receptor { get; }

        private NominaReceptor Nomina { get; }

        public EmployeeComponent(ComprobanteReceptor receptor, NominaReceptor nomina)
        {
            Receptor = receptor;
            Nomina = nomina;
        }

        public void Compose(IContainer container)
        {
            container.ShowEntire().Column(column =>
            {
                column.Spacing(1);

                column.Item().Text(Receptor.Nombre).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).ExtraBold();

                column.Item().Row(row =>
                {
                    row.Spacing(2);

                    row.AutoItem().Text("RFC:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Receptor.Rfc).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                    row.AutoItem().Text("CURP:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.Curp).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("Régimen Fiscal:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Receptor.RegimenFiscalReceptor.ToString()).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("Domicilio:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Receptor.DomicilioFiscalReceptor).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                    row.AutoItem().Text("Entidad Federativa:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.ClaveEntFed).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("NSS:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.NumSeguridadSocial).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                    row.AutoItem().Text("N° de Empleado:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.NumEmpleado).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("Departamento:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.Departamento).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("Puesto:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.Puesto).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });

                column.Item().Row(row =>
                {
                    row.Spacing(2);
                    row.AutoItem().Text("Inicio Laboral:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.FechaInicioRelLaboral.ToString("yyyy-MM-dd")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                    row.AutoItem().Text("Antigüedad:").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    row.AutoItem().Text(Nomina.Antigüedad).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                });
            });
        }
    }

    public class PerceptionsComponent : IComponent
    {
        public NominaPercepciones Percepciones { get; }

        public PerceptionsComponent(NominaPercepciones percepciones)
        {
            Percepciones = percepciones;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Tipo").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Concepto").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Exento").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Gravado").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();

                    static IContainer CellStyle(IContainer container) => container.Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingLeft(5);
                });

                foreach (NominaPercepcionesPercepcion percepcion in Percepciones.Percepcion)
                {
                    table.Cell().Element(CellStyle).Text(percepcion.TipoPercepcion).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                    table.Cell().Element(CellStyle).Text(percepcion.Concepto).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                    table.Cell().Element(CellStyle).Text(percepcion.ImporteExento.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                    table.Cell().Element(CellStyle).Text(percepcion.ImporteGravado.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);

                    static IContainer CellStyle(IContainer container) => container.BorderBottom(0.75F).BorderColor(Colors.Grey.Lighten2).PaddingVertical(1.5F).PaddingLeft(5);
                }

                table.Footer(footer =>
                {
                    footer.Cell().ColumnSpan(2).Element(CellStyle).PaddingRight(10).Text("Total").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold().AlignEnd();
                    footer.Cell().Element(CellStyle).Text(Percepciones.TotalExento.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    footer.Cell().Element(CellStyle).Text(Percepciones.TotalGravado.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();

                    static IContainer CellStyle(IContainer container) => container.Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingLeft(5);
                });
            });
        }
    }

    public class DeductionsComponent : IComponent
    {
        public string Descuento { get; }

        public NominaDeducciones Deducciones { get; }

        public DeductionsComponent(string descuento, NominaDeducciones deducciones)
        {
            Descuento = descuento;
            Deducciones = deducciones;
        }

        public void Compose(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Tipo").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Descripción").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();
                    header.Cell().Element(CellStyle).Text("Importe").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();

                    static IContainer CellStyle(IContainer container) => container.Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingLeft(5);
                });

                if (Deducciones != null)
                {
                    foreach (NominaDeduccionesDeduccion deduccion in Deducciones.Deduccion)
                    {
                        table.Cell().Element(CellStyle).Text(deduccion.TipoDeduccion).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                        table.Cell().Element(CellStyle).Text(deduccion.Concepto).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);
                        table.Cell().Element(CellStyle).Text(deduccion.Importe.ToString("C")).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken2);

                        static IContainer CellStyle(IContainer container) => container.BorderBottom(0.75F).BorderColor(Colors.Grey.Lighten2).PaddingVertical(1.5F).PaddingLeft(5);
                    }
                }

                table.Footer(footer =>
                {
                    footer.Cell().ColumnSpan(2).Element(CellStyle).PaddingRight(10).Text("Total").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold().AlignEnd();
                    footer.Cell().Element(CellStyle).Text(Descuento).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Grey.Darken3).Bold();

                    static IContainer CellStyle(IContainer container) => container.Background(Colors.Grey.Lighten3).PaddingVertical(3).PaddingLeft(5);
                });
            });
        }
    }
}
