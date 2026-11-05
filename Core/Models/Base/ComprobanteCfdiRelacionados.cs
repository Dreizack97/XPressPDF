namespace Core.Models.Base;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
public partial class ComprobanteCfdiRelacionados
{
    [XmlElement("CfdiRelacionado")]
    public ComprobanteCfdiRelacionadosCfdiRelacionado[] CfdiRelacionado { get; set; } = [];

    [XmlAttribute]
    public string TipoRelacion { get; set; } = string.Empty;
}
