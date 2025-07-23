using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaPercepcionesPercepcionAccionesOTitulos
    {
        [XmlAttribute]
        public decimal ValorMercado { get; set; }

        [XmlAttribute]
        public decimal PrecioAlOtorgarse { get; set; }
    }
}
