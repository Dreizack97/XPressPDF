namespace Core.Pdf.Documents;

using Core.Catalogs;
using Core.Common;
using Core.Models.Base;
using Core.Models.Complements.Nomina12;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

/// <summary>
/// Representación impresa de CFDI de Nómina (N) con complemento Nómina 1.2 conforme al Anexo 20 y guías del SAT.
/// </summary>
public class NominaDocument : CfdiDocumentBase
{
    public NominaDocument(Comprobante comprobante, CfdiConversionOptions options)
        : base(comprobante, options)
    {
    }

    protected override string DocumentTitle => "CFDI de Nómina";

    protected override void ComposeContent(IContainer container)
    {
        Nomina? nomina = Comprobante.Nomina;

        container.PaddingVertical(6).Column(column =>
        {
            column.Spacing(6);

            // 1. Datos de Emisor y Trabajador
            column.Item().Row(row =>
            {
                row.Spacing(10);

                // Empleador
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
                    });
                });

                // Trabajador
                row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Datos del Trabajador").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    col.Item().Padding(4).Column(content =>
                    {
                        content.Spacing(1);
                        content.Item().Text(Comprobante.Receptor.Nombre).Style(CfdiPdfTheme.LabelStyle);

                        content.Item().Row(r =>
                        {
                            r.Spacing(4);
                            r.AutoItem().Text("RFC:").Style(CfdiPdfTheme.LabelStyle);
                            r.RelativeItem().Text(Comprobante.Receptor.Rfc).Style(CfdiPdfTheme.ValueStyle);
                            r.AutoItem().Text("CURP:").Style(CfdiPdfTheme.LabelStyle);
                            r.RelativeItem().Text(nomina?.Receptor.Curp ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
                        });

                        content.Item().Row(r =>
                        {
                            r.Spacing(4);
                            r.AutoItem().Text("NSS:").Style(CfdiPdfTheme.LabelStyle);
                            r.RelativeItem().Text(nomina?.Receptor.NumSeguridadSocial ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
                            r.AutoItem().Text("No. Empleado:").Style(CfdiPdfTheme.LabelStyle);
                            r.RelativeItem().Text(nomina?.Receptor.NumEmpleado ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
                        });

                        LabeledRow(content, "Régimen:", SatCatalogProvider.GetTipoRegimen(nomina?.Receptor.TipoRegimen));
                        LabeledRow(content, "Puesto / Depto:", $"{nomina?.Receptor.Puesto ?? "N/A"} / {nomina?.Receptor.Departamento ?? "N/A"}");

                        content.Item().Row(r =>
                        {
                            r.Spacing(4);
                            r.AutoItem().Text("Contrato:").Style(CfdiPdfTheme.LabelStyle);
                            r.RelativeItem().Text(SatCatalogProvider.GetTipoContrato(nomina?.Receptor.TipoContrato)).Style(CfdiPdfTheme.ValueStyle);
                            r.AutoItem().Text("Periodicidad:").Style(CfdiPdfTheme.LabelStyle);
                            r.RelativeItem().Text(SatCatalogProvider.GetPeriodicidadPago(nomina?.Receptor.PeriodicidadPago)).Style(CfdiPdfTheme.ValueStyle);
                        });

                        if (nomina?.Receptor.FechaInicioRelLaboralSpecified == true)
                        {
                            content.Item().Row(r =>
                            {
                                r.Spacing(4);
                                r.AutoItem().Text("Inicio Laboral:").Style(CfdiPdfTheme.LabelStyle);
                                r.RelativeItem().Text(nomina.Receptor.FechaInicioRelLaboral.ToString("yyyy-MM-dd")).Style(CfdiPdfTheme.ValueStyle);
                                r.AutoItem().Text("Antigüedad:").Style(CfdiPdfTheme.LabelStyle);
                                r.RelativeItem().Text(nomina.Receptor.Antigüedad ?? "N/A").Style(CfdiPdfTheme.ValueStyle);
                            });
                        }
                    });
                });
            });

            // 2. Percepciones y Deducciones
            column.Item().Row(row =>
            {
                row.Spacing(10);

                // Percepciones
                row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Percepciones").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    NominaPercepcionesPercepcion[] percepciones = nomina?.Percepciones?.Percepcion ?? [];

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);  // Tipo
                            columns.RelativeColumn(2.5F);// Concepto
                            columns.RelativeColumn(1.0F);// Gravado
                            columns.RelativeColumn(1.0F);// Exento
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tipo").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Concepto").Style(CfdiPdfTheme.LabelStyle);
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Gravado").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Exento").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });

                        foreach (NominaPercepcionesPercepcion p in percepciones)
                        {
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(p.TipoPercepcion).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(p.Concepto).Style(CfdiPdfTheme.ValueStyle);
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(p.ImporteGravado)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                            table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(FormatMoney(p.ImporteExento)).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        }

                        table.Footer(footer =>
                        {
                            decimal totalGravado = nomina?.Percepciones?.TotalGravado ?? 0;
                            decimal totalExento = nomina?.Percepciones?.TotalExento ?? 0;

                            footer.Cell().ColumnSpan(2).Element(CfdiPdfTheme.CellHeaderStyle).Text("Total Percepciones").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            footer.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text(FormatMoney(totalGravado)).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                            footer.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text(FormatMoney(totalExento)).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        });
                    });
                });

                // Deducciones
                row.RelativeItem().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
                {
                    col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                        .Text("Deducciones").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

                    NominaDeduccionesDeduccion[] deducciones = nomina?.Deducciones?.Deduccion ?? [];

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

            // 3. Otros Pagos (Solución a la omisión del sistema legado)
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

            // 5. Resumen del Periodo y Totales
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
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Días").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("SBC").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("SDI").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Percepciones").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Deducciones").Style(CfdiPdfTheme.LabelStyle);
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
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dias).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(sbc).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(sdi).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(percepcionesTotal).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(deduccionesTotal).Style(CfdiPdfTheme.ValueStyle);
                    table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(neto).Style(CfdiPdfTheme.LabelStyle).AlignRight();
                });
            });

            // 6. Recibo de conformidad y firma del empleado
            column.Item().PaddingTop(10).Row(row =>
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
        });
    }

    private static void LabeledRow(ColumnDescriptor col, string label, string? value)
    {
        col.Item().Row(row =>
        {
            row.Spacing(2);
            row.AutoItem().Text(label).Style(CfdiPdfTheme.LabelStyle);
            row.RelativeItem().Text(value).Style(CfdiPdfTheme.ValueStyle);
        });
    }
}
