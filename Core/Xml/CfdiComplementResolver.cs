namespace Core.Xml;

using Core.Abstractions;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Resuelve e instancía los complementos del CFDI a partir de nodos XmlElement utilizando XmlNodeReader de alto rendimiento.
/// </summary>
public class CfdiComplementResolver : ICfdiComplementResolver
{
    private readonly CfdiComplementRegistry _registry;

    public CfdiComplementResolver(CfdiComplementRegistry registry)
    {
        _registry = registry;
    }

    public IReadOnlyList<ICfdiComplement> ResolveComplements(IEnumerable<XmlElement> elements)
    {
        List<ICfdiComplement> results = [];

        foreach (XmlElement element in elements)
        {
            string ns = element.NamespaceURI;
            string localName = element.LocalName;

            if (_registry.TryGetComplementType(ns, localName, out Type? targetType) && targetType != null)
            {
                try
                {
                    using XmlNodeReader reader = new(element);
                    XmlSerializer serializer = XmlSerializerCache.GetSerializer(targetType);

                    if (serializer.Deserialize(reader) is ICfdiComplement complement)
                    {
                        results.Add(complement);
                    }
                }
                catch
                {
                    // Se omite si el nodo particular tiene inconsistencias, permitiendo procesar el resto
                }
            }
        }

        return results;
    }

    public IReadOnlyList<ICfdiConceptComplement> ResolveConceptComplements(IEnumerable<XmlElement> elements)
    {
        List<ICfdiConceptComplement> results = [];

        foreach (XmlElement element in elements)
        {
            string ns = element.NamespaceURI;
            string localName = element.LocalName;

            if (_registry.TryGetConceptComplementType(ns, localName, out Type? targetType) && targetType != null)
            {
                try
                {
                    using XmlNodeReader reader = new(element);
                    XmlSerializer serializer = XmlSerializerCache.GetSerializer(targetType);

                    if (serializer.Deserialize(reader) is ICfdiConceptComplement conceptComplement)
                    {
                        results.Add(conceptComplement);
                    }
                }
                catch
                {
                    // Se omite si el nodo particular tiene inconsistencias
                }
            }
        }

        return results;
    }
}
