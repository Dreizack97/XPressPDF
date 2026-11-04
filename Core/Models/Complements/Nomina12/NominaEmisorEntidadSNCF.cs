namespace Core.Models.Complements.Nomina12;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public partial class NominaEmisorEntidadSNCF
{
    [XmlAttribute]
    public string OrigenRecurso { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal MontoRecursoPropio { get; set; }

    [XmlIgnore]
    public bool MontoRecursoPropioSpecified { get; set; }
}
