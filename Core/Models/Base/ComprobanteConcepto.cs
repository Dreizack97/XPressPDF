namespace Core.Models.Base;

using Core.Abstractions;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
public partial class ComprobanteConcepto
{
    public ComprobanteConceptoImpuestos? Impuestos { get; set; }

    public ComprobanteConceptoACuentaTerceros? ACuentaTerceros { get; set; }

    [XmlElement("InformacionAduanera")]
    public ComprobanteConceptoInformacionAduanera[] InformacionAduanera { get; set; } = [];

    [XmlElement("CuentaPredial")]
    public ComprobanteConceptoCuentaPredial[] CuentaPredial { get; set; } = [];

    public ComprobanteConceptoComplementoConcepto? ComplementoConcepto { get; set; }

    [XmlElement("Parte")]
    public ComprobanteConceptoParte[] Parte { get; set; } = [];

    // --- Complementos de Concepto Desacoplados ---
    [XmlIgnore]
    public IReadOnlyList<ICfdiConceptComplement> ConceptComplements { get; internal set; } = [];

    public T? GetConceptComplement<T>() where T : class, ICfdiConceptComplement =>
        ConceptComplements.OfType<T>().FirstOrDefault();

    public bool TryGetConceptComplement<T>([NotNullWhen(true)] out T? conceptComplement) where T : class, ICfdiConceptComplement
    {
        conceptComplement = GetConceptComplement<T>();
        return conceptComplement is not null;
    }

    [XmlAttribute]
    public string ClaveProdServ { get; set; } = string.Empty;

    [XmlAttribute]
    public string NoIdentificacion { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal Cantidad { get; set; }

    [XmlAttribute]
    public string ClaveUnidad { get; set; } = string.Empty;

    [XmlAttribute]
    public string Unidad { get; set; } = string.Empty;

    [XmlAttribute]
    public string Descripcion { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal ValorUnitario { get; set; }

    [XmlAttribute]
    public decimal Importe { get; set; }

    [XmlAttribute]
    public decimal Descuento { get; set; }

    [XmlIgnore]
    public bool DescuentoSpecified { get; set; }

    [XmlAttribute]
    public string ObjetoImp { get; set; } = "01";
}
