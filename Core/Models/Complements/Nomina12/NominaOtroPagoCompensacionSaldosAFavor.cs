namespace Core.Models.Complements.Nomina12;

using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
public partial class NominaOtroPagoCompensacionSaldosAFavor
{
    [XmlAttribute]
    public decimal SaldoAFavor { get; set; }

    [XmlAttribute]
    public short Año { get; set; }

    [XmlAttribute]
    public decimal RemanenteSalFav { get; set; }
}
