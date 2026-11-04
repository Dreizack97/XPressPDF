namespace Core.Xml;

using Core.Abstractions;
using Core.Models.Complements.CartaPorte31;
using Core.Models.Complements.ComercioExterior20;
using Core.Models.Complements.Divisas10;
using Core.Models.Complements.ImpuestosLocales10;
using Core.Models.Complements.LeyendasFiscales10;
using Core.Models.Complements.Nomina12;
using Core.Models.Complements.Pagos20;
using Core.Models.Complements.TimbreFiscalDigital;
using Core.Models.Complements.ValesDeDespensa10;
using Core.Models.ConceptComplements.InstitucionesEducativas10;
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
        // Registro de complementos de comprobante
        RegisterComplement<TimbreFiscalDigital>("http://www.sat.gob.mx/TimbreFiscalDigital", "TimbreFiscalDigital");
        RegisterComplement<Nomina>("http://www.sat.gob.mx/nomina12", "Nomina");
        RegisterComplement<ValesDeDespensa>("http://www.sat.gob.mx/valesdedespensa", "ValesDeDespensa");
        RegisterComplement<Pagos>("http://www.sat.gob.mx/Pagos20", "Pagos");
        RegisterComplement<CartaPorte>("http://www.sat.gob.mx/CartaPorte31", "CartaPorte");
        RegisterComplement<ImpuestosLocales>("http://www.sat.gob.mx/implocal", "ImpuestosLocales");
        RegisterComplement<LeyendasFiscales>("http://www.sat.gob.mx/leyendasFiscales", "LeyendasFiscales");
        RegisterComplement<ComercioExterior>("http://www.sat.gob.mx/ComercioExterior20", "ComercioExterior");
        RegisterComplement<Divisas>("http://www.sat.gob.mx/divisas", "Divisas");

        // Registro de complementos a nivel concepto
        RegisterConceptComplement<InstEducativas>("http://www.sat.gob.mx/iedu", "instEducativas");
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
