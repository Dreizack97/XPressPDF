namespace Core.Models.Base;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
public partial class ComprobanteInformacionGlobal
{
    [XmlAttribute]
    public string Periodicidad { get; set; } = string.Empty;

    [XmlAttribute]
    public string Meses { get; set; } = string.Empty;

    [XmlAttribute]
    public short Año { get; set; }
}
