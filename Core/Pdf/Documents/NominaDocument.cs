namespace Core.Pdf.Documents;

using Core.Abstractions;
using Core.Catalogs;
using Core.Common;
using Core.Models.Base;
using Core.Models.Complements.Nomina12;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Representación impresa de CFDI de Nómina (N) con complemento Nómina 1.2 conforme al Anexo 20 y guías del SAT.
/// </summary>
public class NominaDocument : CfdiDocumentBase
{
    private readonly IEnumerable<ICfdiComplementSectionRenderer> _complementRenderers;

    public NominaDocument(
        Comprobante comprobante,
        CfdiConversionOptions options,
        IEnumerable<ICfdiComplementSectionRenderer>? complementRenderers = null)
        : base(comprobante, options)
    {
        _complementRenderers = (complementRenderers ?? []).OrderBy(r => r.Order);
    }

    protected override string DocumentTitle => "CFDI de Nómina";

    protected override void ComposeContent(IContainer container)
    {
        Nomina? nomina = Comprobante.Nomina;

        container.PaddingVertical(6).Column(column =>
        {
            column.Spacing(6);

            // 1. Datos de Emisor y Trabajador (Tarjetas simétricas con alineación en rejilla)
            column.Item().Row(row =>
            {
                row.Spacing(10);

                // Empleador (Patrón / Emisor)
                row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Datos del Patrón / Emisor").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Padding(4).Column(content =>
                    {
                        content.Spacing(1);
                        content.Item().Text(Comprobante.Emisor.Nombre).Style(CfdiPdfTheme.LabelStyle);
                        LabeledRow(content, "RFC:", Comprobante.Emisor.Rfc);
                        LabeledRow(content, "Régimen Fiscal:", SatCatalogProvider.GetRegimenFiscal(Comprobante.Emisor.RegimenFiscal));

                        if (!string.IsNullOrWhiteSpace(nomina?.Emisor?.RegistroPatronal))
                        {
                            LabeledRow(content, "Registro Patronal:", nomina.Emisor.RegistroPatronal);
                        }

                        if (!string.IsNullOrWhiteSpace(nomina?.Emisor?.RfcPatronOrigen))
                        {
                            LabeledRow(content, "RFC Patrón Origen:", nomina.Emisor.RfcPatronOrigen);
                        }

                        if (!string.IsNullOrWhiteSpace(nomina?.TipoNomina))
                        {
                            LabeledRow(content, "Tipo de Nómina:", SatCatalogProvider.GetTipoNomina(nomina.TipoNomina));
                        }
                    });
                });

                // Trabajador (Receptor)
                row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Datos del Trabajador").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Padding(4).Column(content =>
                    {
                        content.Spacing(1);
                        content.Item().Text(Comprobante.Receptor.Nombre).Style(CfdiPdfTheme.LabelStyle);

                        // Fila 1: RFC | CURP
                        TwoColumnRow(content, "RFC:", Comprobante.Receptor.Rfc, "CURP:", nomina?.Receptor.Curp);

                        // Fila 2: NSS | No. Empleado
                        TwoColumnRow(content, "NSS:", nomina?.Receptor.NumSeguridadSocial, "No. Empleado:", nomina?.Receptor.NumEmpleado);

                        // Fila 3: Domicilio Fiscal | Periodicidad
                        string dom = !string.IsNullOrWhiteSpace(nomina?.Receptor.ClaveEntFed)
                            ? $"{Comprobante.Receptor.DomicilioFiscalReceptor} ({nomina.Receptor.ClaveEntFed})"
                            : Comprobante.Receptor.DomicilioFiscalReceptor;
                        TwoColumnRow(content, "Domicilio Fiscal:", dom, "Periodicidad:", SatCatalogProvider.GetPeriodicidadPago(nomina?.Receptor.PeriodicidadPago));

                        // Fila 4: Régimen Fiscal (Clave y Descripción completa del catálogo del SAT)
                        LabeledRow(content, "Régimen Fiscal:", SatCatalogProvider.GetRegimenFiscal(Comprobante.Receptor.RegimenFiscalReceptor));

                        // Fila 5: Régimen de Contratación (Línea completa para evitar saltos desalineados)
                        LabeledRow(content, "Régimen Contratación:", SatCatalogProvider.GetTipoRegimen(nomina?.Receptor.TipoRegimen));

                        // Fila 6: Tipo de Contrato (Línea completa para evitar truncamientos)
                        LabeledRow(content, "Contrato:", SatCatalogProvider.GetTipoContrato(nomina?.Receptor.TipoContrato));

                        // Fila 7: Puesto | Departamento
                        TwoColumnRow(content, "Puesto:", nomina?.Receptor.Puesto, "Depto:", nomina?.Receptor.Departamento ?? "Ninguno");

                        // Fila 8: Inicio Laboral | Antigüedad (si aplica)
                        if (nomina?.Receptor.FechaInicioRelLaboralSpecified == true || !string.IsNullOrWhiteSpace(nomina?.Receptor.Antigüedad))
                        {
                            string inicio = nomina?.Receptor.FechaInicioRelLaboralSpecified == true
                                ? nomina.Receptor.FechaInicioRelLaboral.ToString("yyyy-MM-dd")
                                : "N/A";
                            TwoColumnRow(content, "Inicio Laboral:", inicio, "Antigüedad:", nomina?.Receptor.Antigüedad);
                        }

                        // Fila 9: Tipo de Jornada (si aplica)
                        if (!string.IsNullOrWhiteSpace(nomina?.Receptor.TipoJornada))
                        {
                            LabeledRow(content, "Tipo de Jornada:", SatCatalogProvider.GetTipoJornada(nomina.Receptor.TipoJornada));
                        }

                        // Fila 10: Banco y Cuenta Bancaria (si aplica)
                        if (!string.IsNullOrWhiteSpace(nomina?.Receptor.Banco) || !string.IsNullOrWhiteSpace(nomina?.Receptor.CuentaBancaria))
                        {
                            TwoColumnRow(content, "Banco:", SatCatalogProvider.GetBanco(nomina?.Receptor.Banco), "Cuenta:", nomina?.Receptor.CuentaBancaria);
                        }
                    });
                });
            });

            // 2. Percepciones y Deducciones (Simetría de filas para eliminar áreas blancas)
            NominaPercepcionesPercepcion[] percepciones = nomina?.Percepciones?.Percepcion ?? [];
            NominaDeduccionesDeduccion[] deducciones = nomina?.Deducciones?.Deduccion ?? [];
            int maxConceptRows = Math.Max(Math.Max(percepciones.Length, deducciones.Length), 1);

            column.Item().Row(row =>
            {
                row.Spacing(10);

                // Percepciones: Orden Legado (Tipo | Concepto | Exento | Gravado)
                row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Percepciones").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);  // Tipo
                            columns.RelativeColumn(2.5F);// Concepto
                            columns.RelativeColumn(1.0F);// Exento
                            columns.RelativeColumn(1.0F);// Gravado
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tipo").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Concepto").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Exento").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Gravado").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });

                        foreach (NominaPercepcionesPercepcion p in percepciones)
                        {
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(p.TipoPercepcion).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(p.Concepto).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(p.ImporteExento)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(p.ImporteGravado)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        }

                        // Filas de relleno para igualar la altura con la tabla de deducciones
                        for (int i = percepciones.Length; i < maxConceptRows; i++)
                        {
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(" ").Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(" ").Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(" ").Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(" ").Style(CfdiPdfTheme.ValueStyle);
                        }

                        table.Footer(footer =>
                        {
                            decimal totalGravado = nomina?.Percepciones?.TotalGravado ?? 0;
                            decimal totalExento = nomina?.Percepciones?.TotalExento ?? 0;

                            footer.Cell().ColumnSpan(2).Element(CfdiPdfTheme.CellHeaderStyle).Text("Total Percepciones").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            footer.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text(FormatMoney(totalExento)).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            footer.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text(FormatMoney(totalGravado)).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });
                    });
                });

                // Deducciones
                row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Deducciones").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);  // Tipo
                            columns.RelativeColumn(3.0F);// Concepto
                            columns.RelativeColumn(1.2F);// Importe
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tipo").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Concepto").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Importe").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });

                        foreach (NominaDeduccionesDeduccion d in deducciones)
                        {
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(d.TipoDeduccion).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(d.Concepto).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(d.Importe)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        }

                        // Filas de relleno para igualar la altura con la tabla de percepciones
                        for (int i = deducciones.Length; i < maxConceptRows; i++)
                        {
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(" ").Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(" ").Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(" ").Style(CfdiPdfTheme.ValueStyle);
                        }

                        table.Footer(footer =>
                        {
                            decimal totalDeducciones = nomina?.TotalDeduccionesSpecified == true
                                ? nomina.TotalDeducciones
                                : Comprobante.Descuento;

                            footer.Cell().ColumnSpan(2).Element(CfdiPdfTheme.CellHeaderStyle).Text("Total Deducciones").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            footer.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text(FormatMoney(totalDeducciones)).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });
                    });
                });
            });

            // 3. Otros Pagos (Mantenido visible conforme a la decisión del usuario)
            NominaOtroPago[] otrosPagos = nomina?.OtrosPagos ?? [];
            if (otrosPagos.Length > 0)
            {
                column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Otros Pagos (Subsidio al Empleo, Compensaciones y Viáticos)").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn(3.5F);
                            columns.RelativeColumn(1.0F);
                            columns.RelativeColumn(1.0F);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tipo").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Concepto").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Detalle").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Importe").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });

                        foreach (NominaOtroPago op in otrosPagos)
                        {
                            string detalle = op.SubsidioAlEmpleo != null
                                ? $"Subsidio Causado: {FormatMoney(op.SubsidioAlEmpleo.SubsidioCausado)}"
                                : string.Empty;

                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(op.TipoOtroPago).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(op.Concepto).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(detalle).Style(CfdiPdfTheme.SmallValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(op.Importe)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        }

                        table.Footer(footer =>
                        {
                            decimal totalOtrosPagos = nomina?.TotalOtrosPagosSpecified == true
                                ? nomina.TotalOtrosPagos
                                : otrosPagos.Sum(op => op.Importe);

                            footer.Cell().ColumnSpan(3).Element(CfdiPdfTheme.CellHeaderStyle).Text("Total Otros Pagos").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            footer.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text(FormatMoney(totalOtrosPagos)).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });
                    });
                });
            }

            // 4. Incapacidades (si existen)
            NominaIncapacidad[] incapacidades = nomina?.Incapacidades ?? [];
            if (incapacidades.Length > 0)
            {
                column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Incapacidades").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(80);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Días").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tipo de Incapacidad").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Descuento").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });

                        foreach (NominaIncapacidad inc in incapacidades)
                        {
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(inc.DiasIncapacidad.ToString()).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(inc.TipoIncapacidad).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(inc.ImporteMonetarioSpecified ? FormatMoney(inc.ImporteMonetario) : "N/A").Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        }
                    });
                });
            }

            // 5. Resumen de Nómina y Salarios
            column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
            {
                col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                    .Text("Resumen de Nómina y Salarios").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.ConstantColumn(40);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn(1.2F);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Fecha Inicio").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Fecha Fin").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Fecha Pago").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Días").Style(CfdiPdfTheme.LabelStyle).AlignCenter();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("SBC").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("SDI").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Percepciones").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Deducciones").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Neto a Pagar").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                    });

                    string fechaIni = nomina?.FechaInicialPago.ToString("yyyy-MM-dd") ?? "N/A";
                    string fechaFin = nomina?.FechaFinalPago.ToString("yyyy-MM-dd") ?? "N/A";
                    string fechaPago = nomina?.FechaPago.ToString("yyyy-MM-dd") ?? "N/A";
                    string dias = nomina?.NumDiasPagados.ToString("0.##") ?? "0";
                    string sbc = nomina?.Receptor.SalarioBaseCotAporSpecified == true ? FormatMoney(nomina.Receptor.SalarioBaseCotApor) : "N/A";
                    string sdi = nomina?.Receptor.SalarioDiarioIntegradoSpecified == true ? FormatMoney(nomina.Receptor.SalarioDiarioIntegrado) : "N/A";
                    string percepcionesTotal = FormatMoney(Comprobante.SubTotal);
                    string deduccionesTotal = FormatMoney(Comprobante.Descuento);
                    string neto = $"{FormatMoney(Comprobante.Total)} {Comprobante.Moneda}";

                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(fechaIni).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(fechaFin).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(fechaPago).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dias).Style(CfdiPdfTheme.ValueStyle).AlignCenter();
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(sbc).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(sdi).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(percepcionesTotal).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(deduccionesTotal).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(neto).Style(CfdiPdfTheme.ValueBoldStyle).AlignRight();
                });
            });

            // 6. Recibo de conformidad y firma del empleado
            column.Item().PaddingTop(8).Row(row =>
            {
                row.Spacing(15);

                row.RelativeItem(65).Text(
                    $"RECIBÍ DE {Comprobante.Emisor.Nombre} LA CANTIDAD DE {FormatMoney(Comprobante.Total)} {Comprobante.Moneda} POR CONCEPTO DE MIS PERCEPCIONES " +
                    "Y PRESTACIONES DEVENGADAS EN EL PERIODO INDICADO, NO EXISTIENDO NINGÚN ADEUDO NI RECLAMO PENDIENTE.").Style(CfdiPdfTheme.SmallValueStyle).Justify();

                row.RelativeItem(35).Column(col =>
                {
                    col.Item().PaddingTop(15).BorderTop(0.75F).BorderColor(CfdiPdfTheme.SecondaryColor).AlignCenter()
                        .Text("Firma del Trabajador").Style(CfdiPdfTheme.LabelStyle);
                });
            });

            // Complementos modulares inyectados
            foreach (ICfdiComplementSectionRenderer renderer in _complementRenderers)
            {
                if (renderer.CanRender(Comprobante))
                {
                    renderer.Render(column, Comprobante);
                }
            }
        });
    }

    private static void LabeledRow(ColumnDescriptor col, string label, string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return;
        col.Item().Row(row =>
        {
            row.Spacing(2);
            row.AutoItem().Text(label).Style(CfdiPdfTheme.LabelStyle);
            row.RelativeItem().Text(value).Style(CfdiPdfTheme.ValueStyle);
        });
    }

    private static void TwoColumnRow(ColumnDescriptor col, string label1, string? value1, string label2, string? value2)
    {
        col.Item().Row(row =>
        {
            row.Spacing(8);
            row.RelativeItem().Row(r1 =>
            {
                r1.Spacing(2);
                r1.AutoItem().Text(label1).Style(CfdiPdfTheme.LabelStyle);
                r1.RelativeItem().Text(value1 ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
            });
            row.RelativeItem().Row(r2 =>
            {
                r2.Spacing(2);
                r2.AutoItem().Text(label2).Style(CfdiPdfTheme.LabelStyle);
                r2.RelativeItem().Text(value2 ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
            });
        });
    }
}
