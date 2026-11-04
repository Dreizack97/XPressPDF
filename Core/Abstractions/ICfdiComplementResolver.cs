namespace Core.Abstractions;

using System.Xml;

/// <summary>
/// Contrato para resolver e instanciar dinámicamente complementos a partir de nodos XmlElement.
/// </summary>
public interface ICfdiComplementResolver
{
    IReadOnlyList<ICfdiComplement> ResolveComplements(IEnumerable<XmlElement> elements);

    IReadOnlyList<ICfdiConceptComplement> ResolveConceptComplements(IEnumerable<XmlElement> elements);
}
