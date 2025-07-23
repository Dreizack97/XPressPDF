using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaPercepcionesPercepcionHorasExtra
    {
        [XmlAttribute]
        public int Dias { get; set; }

        [XmlAttribute]
        public string TipoHoras { get; set; }

        [XmlAttribute]
        public int HorasExtra { get; set; }

        [XmlAttribute]
        public decimal ImportePagado { get; set; }
    }
}
