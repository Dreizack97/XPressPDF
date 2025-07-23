using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaDeducciones
    {
        [XmlElement("Deduccion")]
        public NominaDeduccionesDeduccion[] Deduccion { get; set; }

        [XmlAttribute]
        public decimal TotalOtrasDeducciones { get; set; }

        [XmlIgnore]
        public bool TotalOtrasDeduccionesSpecified { get; set; }

        [XmlAttribute]
        public decimal TotalImpuestosRetenidos { get; set; }

        [XmlIgnore]
        public bool TotalImpuestosRetenidosSpecified { get; set; }
    }
}
