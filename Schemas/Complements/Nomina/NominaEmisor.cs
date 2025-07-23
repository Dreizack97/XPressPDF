using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaEmisor
    {
        public NominaEmisorEntidadSNCF EntidadSNCF { get; set; }

        [XmlAttribute]
        public string Curp { get; set; }

        [XmlAttribute]
        public string RegistroPatronal { get; set; }

        [XmlAttribute]
        public string RfcPatronOrigen { get; set; }
    }
}