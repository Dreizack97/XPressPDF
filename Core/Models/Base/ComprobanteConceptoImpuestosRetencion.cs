namespace Core.Models.Base;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
public partial class ComprobanteConceptoImpuestosRetencion
{
    [XmlAttribute]
    public decimal Base { get; set; }

    [XmlAttribute]
    public string Impuesto { get; set; } = string.Empty;

    [XmlAttribute]
    public string TipoFactor { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal TasaOCuota { get; set; }

    [XmlAttribute]
    public decimal Importe { get; set; }
}
