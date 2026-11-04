namespace Core.Xml;

using Core.Abstractions;
using Core.Models.Complements.Nomina12;
using Core.Models.Complements.TimbreFiscalDigital;
using Core.Models.Complements.ValesDeDespensa10;
using System.Collections.Concurrent;

/// <summary>
/// Registro centralizado y extensible para la asociación de espacios de nombres XML con tipos C# de complementos.
/// </summary>
public class CfdiComplementRegistry
{
    private readonly ConcurrentDictionary<(string Namespace, string LocalName), Type> _complements = new();
    private readonly ConcurrentDictionary<(string Namespace, string LocalName), Type> _conceptComplements = new();

    public CfdiComplementRegistry()
    {
        // Registro de complementos base
        RegisterComplement<TimbreFiscalDigital>("http://www.sat.gob.mx/TimbreFiscalDigital", "TimbreFiscalDigital");
        RegisterComplement<Nomina>("http://www.sat.gob.mx/nomina12", "Nomina");
        RegisterComplement<ValesDeDespensa>("http://www.sat.gob.mx/valesdedespensa", "ValesDeDespensa");
    }

    public void RegisterComplement<T>(string xmlNamespace, string localName) where T : class, ICfdiComplement
    {
        _complements[(xmlNamespace, localName)] = typeof(T);
    }

    public void RegisterConceptComplement<T>(string xmlNamespace, string localName) where T : class, ICfdiConceptComplement
    {
        _conceptComplements[(xmlNamespace, localName)] = typeof(T);
    }

    public bool TryGetComplementType(string xmlNamespace, string localName, out Type? complementType) =>
        _complements.TryGetValue((xmlNamespace, localName), out complementType);

    public bool TryGetConceptComplementType(string xmlNamespace, string localName, out Type? conceptComplementType) =>
        _conceptComplements.TryGetValue((xmlNamespace, localName), out conceptComplementType);
}
