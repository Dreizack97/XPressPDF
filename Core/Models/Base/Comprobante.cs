namespace Core.Models.Base;

using Core.Abstractions;
using Core.Models.Complements.TimbreFiscalDigital;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/cfd/4", IsNullable = false)]
public partial class Comprobante
{
    public ComprobanteInformacionGlobal? InformacionGlobal { get; set; }

    [XmlElement("CfdiRelacionados")]
    public ComprobanteCfdiRelacionados[] CfdiRelacionados { get; set; } = [];

    public ComprobanteEmisor Emisor { get; set; } = null!;

    public ComprobanteReceptor Receptor { get; set; } = null!;

    [XmlArrayItem("Concepto", IsNullable = false)]
    public ComprobanteConcepto[] Conceptos { get; set; } = [];

    public ComprobanteImpuestos? Impuestos { get; set; }

    public ComprobanteComplemento? Complemento { get; set; }

    public ComprobanteAddenda? Addenda { get; set; }

    // --- Contenedor Desacoplado de Complementos (Principios SOLID / OCP) ---
    [XmlIgnore]
    public IReadOnlyList<ICfdiComplement> Complements { get; internal set; } = [];

    public T? GetComplement<T>() where T : class, ICfdiComplement =>
        Complements.OfType<T>().FirstOrDefault();

    public bool TryGetComplement<T>([NotNullWhen(true)] out T? complement) where T : class, ICfdiComplement
    {
        complement = GetComplement<T>();
        return complement is not null;
    }

    // --- Accesos de conveniencia fuertemente tipados ---
    [XmlIgnore]
    public TimbreFiscalDigital? TimbreFiscalDigital => GetComplement<TimbreFiscalDigital>();

    // --- Atributos XML Oficiales del Comprobante (Anexo 20 CFDI 4.0) ---
    [XmlAttribute]
    public string Version { get; set; } = "4.0";

    [XmlAttribute]
    public string Serie { get; set; } = string.Empty;

    [XmlAttribute]
    public string Folio { get; set; } = string.Empty;

    [XmlAttribute]
    public DateTime Fecha { get; set; }

    [XmlAttribute]
    public string Sello { get; set; } = string.Empty;

    [XmlAttribute]
    public string? FormaPago { get; set; }

    [XmlIgnore]
    public bool FormaPagoSpecified { get; set; }

    [XmlAttribute]
    public string NoCertificado { get; set; } = string.Empty;

    [XmlAttribute]
    public string Certificado { get; set; } = string.Empty;

    [XmlAttribute]
    public string CondicionesDePago { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal SubTotal { get; set; }

    [XmlAttribute]
    public decimal Descuento { get; set; }

    [XmlIgnore]
    public bool DescuentoSpecified { get; set; }

    [XmlAttribute]
    public string Moneda { get; set; } = "MXN";

    [XmlAttribute]
    public decimal TipoCambio { get; set; }

    [XmlIgnore]
    public bool TipoCambioSpecified { get; set; }

    [XmlAttribute]
    public decimal Total { get; set; }

    [XmlAttribute]
    public string TipoDeComprobante { get; set; } = "I";

    [XmlAttribute]
    public string Exportacion { get; set; } = "01";

    [XmlAttribute]
    public string? MetodoPago { get; set; }

    [XmlIgnore]
    public bool MetodoPagoSpecified { get; set; }

    [XmlAttribute]
    public string LugarExpedicion { get; set; } = string.Empty;

    [XmlAttribute]
    public string Confirmacion { get; set; } = string.Empty;
}
