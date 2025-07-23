using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaEmisorEntidadSNCF
    {
        [XmlAttribute]
        public string OrigenRecurso { get; set; }

        [XmlAttribute]
        public decimal MontoRecursoPropio { get; set; }

        [XmlIgnore]
        public bool MontoRecursoPropioSpecified { get; set; }
    }
}
