using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaPercepcionesPercepcion
    {
        public NominaPercepcionesPercepcionAccionesOTitulos AccionesOTitulos { get; set; }

        [XmlElement("HorasExtra")]
        public NominaPercepcionesPercepcionHorasExtra[] HorasExtra { get; set; }

        [XmlAttribute]
        public string TipoPercepcion { get; set; }

        [XmlAttribute]
        public string Clave { get; set; }

        [XmlAttribute]
        public string Concepto { get; set; }

        [XmlAttribute]
        public decimal ImporteGravado { get; set; }

        [XmlAttribute]
        public decimal ImporteExento { get; set; }
    }
}
