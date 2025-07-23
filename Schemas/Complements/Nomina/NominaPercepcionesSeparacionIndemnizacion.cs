using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaPercepcionesSeparacionIndemnizacion
    {
        [XmlAttribute]
        public decimal TotalPagado { get; set; }

        [XmlAttribute]
        public int NumAñosServicio { get; set; }

        [XmlAttribute]
        public decimal UltimoSueldoMensOrd { get; set; }

        [XmlAttribute]
        public decimal IngresoAcumulable { get; set; }

        [XmlAttribute]
        public decimal IngresoNoAcumulable { get; set; }
    }
}
