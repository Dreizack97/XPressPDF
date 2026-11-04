namespace Core.Models.Complements.Nomina12;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public partial class NominaPercepcionesPercepcion
{
    public NominaPercepcionesPercepcionAccionesOTitulos? AccionesOTitulos { get; set; }

    [XmlElement("HorasExtra")]
    public NominaPercepcionesPercepcionHorasExtra[] HorasExtra { get; set; } = [];

    [XmlAttribute]
    public string TipoPercepcion { get; set; } = string.Empty;

    [XmlAttribute]
    public string Clave { get; set; } = string.Empty;

    [XmlAttribute]
    public string Concepto { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal ImporteGravado { get; set; }

    [XmlAttribute]
    public decimal ImporteExento { get; set; }
}
