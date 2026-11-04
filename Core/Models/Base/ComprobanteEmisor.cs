namespace Core.Models.Base;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
public partial class ComprobanteEmisor
{
    [XmlAttribute]
    public string Rfc { get; set; } = string.Empty;

    [XmlAttribute]
    public string Nombre { get; set; } = string.Empty;

    [XmlAttribute]
    public string RegimenFiscal { get; set; } = string.Empty;

    [XmlAttribute]
    public string? FacAtrAdquirente { get; set; }
}
