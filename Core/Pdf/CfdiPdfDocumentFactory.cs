namespace Core.Pdf;

using Core.Abstractions;
using Core.Common;
using Core.Models.Base;
using Core.Pdf.Documents;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Fábrica desacoplada que selecciona y compone la plantilla QuestPDF idónea según el tipo de comprobante.
/// </summary>
public class CfdiPdfDocumentFactory : ICfdiPdfDocumentFactory
{
    private readonly List<ICfdiComplementSectionRenderer> _renderers;

    public CfdiPdfDocumentFactory(IEnumerable<ICfdiComplementSectionRenderer> renderers)
    {
        _renderers = renderers.OrderBy(r => r.Order).ToList();
    }

    public IDocument CreateDocument(Comprobante comprobante, CfdiConversionOptions options)
    {
        return comprobante.TipoDeComprobante switch
        {
            "N" => new NominaDocument(comprobante, options, _renderers),
            "P" => new RecepcionPagosDocument(comprobante, options, _renderers),
            "T" => new TrasladoDocument(comprobante, options, _renderers),
            _ => new IngresoEgresoDocument(comprobante, options, _renderers)
        };
    }
}
