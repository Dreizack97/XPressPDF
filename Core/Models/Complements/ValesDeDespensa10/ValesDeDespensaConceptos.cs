namespace Core.Models.Complements.ValesDeDespensa10;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/valesdedespensa")]
public partial class ValesDeDespensaConceptos
{
    [XmlElement("Concepto")]
    public ValesDeDespensaConceptosConcepto[] Concepto { get; set; } = [];
}
