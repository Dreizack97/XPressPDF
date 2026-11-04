namespace Core.Models.Complements.Nomina12;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public partial class NominaPercepcionesJubilacionPensionRetiro
{
    [XmlAttribute]
    public decimal TotalUnaExhibicion { get; set; }

    [XmlIgnore]
    public bool TotalUnaExhibicionSpecified { get; set; }

    [XmlAttribute]
    public decimal TotalParcialidad { get; set; }

    [XmlIgnore]
    public bool TotalParcialidadSpecified { get; set; }

    [XmlAttribute]
    public decimal MontoDiario { get; set; }

    [XmlIgnore]
    public bool MontoDiarioSpecified { get; set; }

    [XmlAttribute]
    public decimal IngresoAcumulable { get; set; }

    [XmlAttribute]
    public decimal IngresoNoAcumulable { get; set; }
}
