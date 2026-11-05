namespace Core.Models.Base;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
public partial class ComprobanteImpuestosRetencion
{
    [XmlAttribute]
    public string Impuesto { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal Importe { get; set; }
}
