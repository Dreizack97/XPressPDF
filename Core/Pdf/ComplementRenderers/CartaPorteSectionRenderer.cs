namespace Core.Pdf.ComplementRenderers;

using Core.Abstractions;
using Core.Models.Base;
using Core.Models.Complements.CartaPorte31;
using Core.Pdf.Styling;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Linq;

/// <summary>
/// Renderizador modular para la sección del complemento de Carta Porte 3.1.
/// </summary>
public class CartaPorteSectionRenderer : ICfdiComplementSectionRenderer
{
    public int Order => 70;

    public bool CanRender(Comprobante comprobante) => comprobante.CartaPorte is not null;

    public void Render(ColumnDescriptor column, Comprobante comprobante)
    {
        CartaPorte? cp = comprobante.CartaPorte;
        if (cp == null)
            return;

        // 1. Datos Generales de Carta Porte
        column.Item().Border(0.75F, CfdiPdfTheme.BorderColor).CornerRadius(3).Column(col =>
        {
            col.Item().Background(CfdiPdfTheme.HeaderBackground).Padding(2.5F)
                .Text("Complemento Carta Porte 3.1").Style(CfdiPdfTheme.SectionTitleStyle).AlignCenter();

            col.Item().Padding(4).Row(row =>
            {
                row.Spacing(12);

                row.RelativeItem(2).Column(c =>
                {
                    c.Spacing(2);
                    c.Item().Text(t => { t.Span("IdCCP: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cp.IdCCP).Style(CfdiPdfTheme.ValueBoldStyle); });
                    c.Item().Text(t => { t.Span("Transp. Internacional: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cp.TranspInternac).Style(CfdiPdfTheme.ValueStyle); });
                });

                row.RelativeItem().Column(c =>
                {
                    c.Spacing(2);
                    if (cp.TotalDistRecSpecified)
                        c.Item().Text(t => { t.Span("Distancia Recorrida: ").Style(CfdiPdfTheme.LabelStyle); t.Span($"{cp.TotalDistRec:N2} km").Style(CfdiPdfTheme.ValueStyle); });
                    if (cp.Mercancias != null)
                        c.Item().Text(t => { t.Span("Total Mercancías: ").Style(CfdiPdfTheme.LabelStyle); t.Span(cp.Mercancias.NumTotalMercancias.ToString()).Style(CfdiPdfTheme.ValueStyle); });
                });
            });

            // 2. Ubicaciones (Origen / Destino)
            if (cp.Ubicaciones.Count > 0)
            {
                col.Item().Background(CfdiPdfTheme.LightBackground).PaddingHorizontal(4).PaddingVertical(1.5F)
                    .Text("Ubicaciones de Origen y Destino").Style(CfdiPdfTheme.LabelStyle);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1.2F); // Tipo
                        columns.RelativeColumn(2.5F); // RFC / Nombre
                        columns.RelativeColumn(2); // Fecha / Hora
                        columns.RelativeColumn(3.5F); // Domicilio
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Tipo Ubicación").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("RFC / Remitente / Destinatario").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Fecha y Hora").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Domicilio").Style(CfdiPdfTheme.LabelStyle);
                    });

                    foreach (CartaPorteUbicacion ubi in cp.Ubicaciones)
                    {
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(ubi.TipoUbicacion).Style(CfdiPdfTheme.ValueBoldStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text($"{ubi.RfcRemitenteDestinatario}\n{ubi.NombreRemitenteDestinatario ?? string.Empty}").Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(ubi.FechaHoraSalidaLlegada.ToString("yyyy-MM-dd HH:mm:ss")).Style(CfdiPdfTheme.ValueStyle);

                        string dom = ubi.Domicilio != null
                            ? $"{ubi.Domicilio.Calle} {ubi.Domicilio.NumeroExterior}, Col. {ubi.Domicilio.Colonia}, {ubi.Domicilio.Municipio}, {ubi.Domicilio.Estado}, CP {ubi.Domicilio.CodigoPostal}"
                            : "N/A";
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(dom).Style(CfdiPdfTheme.ValueStyle);
                    }
                });
            }

            // 3. Mercancías
            if (cp.Mercancias?.Mercancia.Count > 0)
            {
                col.Item().Background(CfdiPdfTheme.LightBackground).PaddingHorizontal(4).PaddingVertical(1.5F)
                    .Text("Bienes y Mercancías Transportadas").Style(CfdiPdfTheme.LabelStyle);

                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1.5F); // Clave SAT
                        columns.RelativeColumn(4); // Descripción
                        columns.RelativeColumn(1.2F); // Cantidad
                        columns.RelativeColumn(1.2F); // Unidad
                        columns.RelativeColumn(1.5F); // Peso (kg)
                        columns.RelativeColumn(1.5F); // Peligroso
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Bienes Transp").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Descripción").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Cantidad").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Unidad").Style(CfdiPdfTheme.LabelStyle);
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Peso (kg)").Style(CfdiPdfTheme.LabelStyle).AlignRight();
                        header.Cell().Element(CfdiPdfTheme.CellHeaderStyle).Text("Mat. Peligroso").Style(CfdiPdfTheme.LabelStyle);
                    });

                    foreach (CartaPorteMercancia merc in cp.Mercancias.Mercancia)
                    {
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(merc.BienesTransp).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(merc.Descripcion).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(merc.Cantidad.ToString("N2")).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(merc.ClaveUnidad).Style(CfdiPdfTheme.ValueStyle);
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(merc.PesoEnKg.ToString("N2")).Style(CfdiPdfTheme.ValueStyle).AlignRight();
                        table.Cell().Element(CfdiPdfTheme.CellBodyStyle).Text(merc.MaterialPeligroso == "Sí" ? $"Sí ({merc.CveMaterialPeligroso})" : "No").Style(CfdiPdfTheme.ValueStyle);
                    }
                });
            }

            // 4. Autotransporte Federal
            if (cp.Mercancias?.Autotransporte != null)
            {
                CartaPorteAutotransporte auto = cp.Mercancias.Autotransporte;
                col.Item().Background(CfdiPdfTheme.LightBackground).PaddingHorizontal(4).PaddingVertical(1.5F)
                    .Text("Identificación del Autotransporte y Seguros").Style(CfdiPdfTheme.LabelStyle);

                col.Item().Padding(4).Row(row =>
                {
                    row.Spacing(12);

                    row.RelativeItem().Column(c =>
                    {
                        c.Spacing(2);
                        c.Item().Text(t => { t.Span("Permiso SCT: ").Style(CfdiPdfTheme.LabelStyle); t.Span($"{auto.PermSCT} - {auto.NumPermisoSCT}").Style(CfdiPdfTheme.ValueStyle); });
                        if (auto.IdentificacionVehicular != null)
                            c.Item().Text(t => { t.Span("Vehículo: ").Style(CfdiPdfTheme.LabelStyle); t.Span($"Config: {auto.IdentificacionVehicular.ConfigVehicular} | Placas: {auto.IdentificacionVehicular.PlacaVM} | Modelo: {auto.IdentificacionVehicular.AnioModeloVM}").Style(CfdiPdfTheme.ValueStyle); });
                    });

                    row.RelativeItem().Column(c =>
                    {
                        c.Spacing(2);
                        if (auto.Seguros != null)
                            c.Item().Text(t => { t.Span("Aseguradora Resp. Civil: ").Style(CfdiPdfTheme.LabelStyle); t.Span($"{auto.Seguros.AseguraRespCivil} (Póliza: {auto.Seguros.PolizaRespCivil})").Style(CfdiPdfTheme.ValueStyle); });
                        if (auto.Remolques.Count > 0)
                            c.Item().Text(t => { t.Span("Remolque(s): ").Style(CfdiPdfTheme.LabelStyle); t.Span(string.Join(", ", auto.Remolques.Select(r => $"{r.SubTipoRem} ({r.Placa})"))).Style(CfdiPdfTheme.ValueStyle); });
                    });
                });
            }

            // 5. Figura Transporte (Operadores)
            if (cp.FiguraTransporte.Count > 0)
            {
                col.Item().Background(CfdiPdfTheme.LightBackground).PaddingHorizontal(4).PaddingVertical(1.5F)
                    .Text("Operadores / Figuras de Transporte").Style(CfdiPdfTheme.LabelStyle);

                col.Item().Padding(4).Column(figCol =>
                {
                    figCol.Spacing(2);
                    foreach (CartaPorteTiposFigura fig in cp.FiguraTransporte)
                    {
                        figCol.Item().Text(t =>
                        {
                            t.DefaultTextStyle(CfdiPdfTheme.ValueStyle);
                            t.Span($"[{fig.TipoFigura}] ").Style(CfdiPdfTheme.LabelStyle);
                            t.Span($"{fig.NombreFigura ?? "N/A"} - RFC: {fig.RfcFigura}");
                            if (!string.IsNullOrWhiteSpace(fig.NumLicencia))
                                t.Span($" - Licencia: {fig.NumLicencia}");
                        });
                    }
                });
            }
        });
    }
}
