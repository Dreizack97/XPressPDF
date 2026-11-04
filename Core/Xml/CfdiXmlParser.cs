namespace Core.Xml;

using Core.Abstractions;
using Core.Models.Base;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Motor de lectura asíncrona y deserialización de comprobantes fiscales digitales CFDI 4.0.
/// </summary>
public class CfdiXmlParser : ICfdiXmlParser
{
    private static readonly XmlReaderSettings ReaderSettings = new()
    {
        Async = true,
        DtdProcessing = DtdProcessing.Prohibit,
        IgnoreComments = true,
        IgnoreWhitespace = true,
        CloseInput = false
    };

    private readonly ICfdiComplementResolver _complementResolver;

    public CfdiXmlParser(ICfdiComplementResolver complementResolver)
    {
        _complementResolver = complementResolver;
    }

    public async Task<Comprobante> ParseFileAsync(string xmlFilePath, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(xmlFilePath);

        if (!File.Exists(xmlFilePath))
            throw new FileNotFoundException($"No se encontró el archivo XML especificado: {xmlFilePath}", xmlFilePath);

        await using FileStream stream = new(
            xmlFilePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true);

        return await ParseAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Comprobante> ParseAsync(Stream xmlStream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(xmlStream);

        cancellationToken.ThrowIfCancellationRequested();

        Comprobante comprobante;
        try
        {
            using XmlReader xmlReader = XmlReader.Create(xmlStream, ReaderSettings);
            XmlSerializer serializer = XmlSerializerCache.GetSerializer<Comprobante>();

            // XmlSerializer.Deserialize no tiene overload asíncrono nativo en el BCL,
            // pero el XmlReader se abre sobre el stream listo
            comprobante = serializer.Deserialize(xmlReader) as Comprobante
                ?? throw new InvalidOperationException("El contenido XML no pudo ser deserializado como un Comprobante CFDI 4.0 válido.");
        }
        catch (XmlException ex)
        {
            throw new InvalidOperationException($"Error de sintaxis en el archivo XML (Línea: {ex.LineNumber}, Posición: {ex.LinePosition}): {ex.Message}", ex);
        }
        catch (InvalidOperationException ex) when (ex.InnerException is not null)
        {
            throw new InvalidOperationException($"Error al deserializar la estructura del CFDI 4.0: {ex.InnerException.Message}", ex);
        }

        // Resolver e inyectar complementos a nivel Comprobante (<cfdi:Complemento>)
        if (comprobante.Complemento?.Any is { Length: > 0 } complementElements)
        {
            comprobante.Complements = _complementResolver.ResolveComplements(complementElements);
        }

        // Resolver e inyectar complementos a nivel Concepto (<cfdi:ComplementoConcepto>)
        if (comprobante.Conceptos is { Length: > 0 } conceptos)
        {
            foreach (ComprobanteConcepto concepto in conceptos)
            {
                if (concepto.ComplementoConcepto?.Any is { Length: > 0 } conceptElements)
                {
                    concepto.ConceptComplements = _complementResolver.ResolveConceptComplements(conceptElements);
                }
            }
        }

        return comprobante;
    }
}
