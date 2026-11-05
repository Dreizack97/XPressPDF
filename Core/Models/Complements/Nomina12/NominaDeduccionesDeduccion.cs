namespace Core.Models.Complements.Nomina12;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public partial class NominaDeduccionesDeduccion
{
    [XmlAttribute]
    public string TipoDeduccion { get; set; } = string.Empty;

    [XmlAttribute]
    public string Clave { get; set; } = string.Empty;

    [XmlAttribute]
    public string Concepto { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal Importe { get; set; }
}
