namespace Core.Models.Complements.Nomina12;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public partial class NominaPercepciones
{
    [XmlElement("Percepcion")]
    public NominaPercepcionesPercepcion[] Percepcion { get; set; } = [];

    public NominaPercepcionesJubilacionPensionRetiro? JubilacionPensionRetiro { get; set; }

    public NominaPercepcionesSeparacionIndemnizacion? SeparacionIndemnizacion { get; set; }

    [XmlAttribute]
    public decimal TotalSueldos { get; set; }

    [XmlIgnore]
    public bool TotalSueldosSpecified { get; set; }

    [XmlAttribute]
    public decimal TotalSeparacionIndemnizacion { get; set; }

    [XmlIgnore]
    public bool TotalSeparacionIndemnizacionSpecified { get; set; }

    [XmlAttribute]
    public decimal TotalJubilacionPensionRetiro { get; set; }

    [XmlIgnore]
    public bool TotalJubilacionPensionRetiroSpecified { get; set; }

    [XmlAttribute]
    public decimal TotalGravado { get; set; }

    [XmlAttribute]
    public decimal TotalExento { get; set; }
}
