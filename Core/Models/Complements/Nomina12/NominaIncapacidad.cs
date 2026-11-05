namespace Core.Models.Complements.Nomina12;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public partial class NominaIncapacidad
{
    [XmlAttribute]
    public int DiasIncapacidad { get; set; }

    [XmlAttribute]
    public string TipoIncapacidad { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal ImporteMonetario { get; set; }

    [XmlIgnore]
    public bool ImporteMonetarioSpecified { get; set; }
}
