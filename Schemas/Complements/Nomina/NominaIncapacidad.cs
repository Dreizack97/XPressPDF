using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    public partial class NominaIncapacidad
    {
        [XmlAttribute]
        public int DiasIncapacidad { get; set; }

        [XmlAttribute]
        public string TipoIncapacidad { get; set; }

        [XmlAttribute]
        public decimal ImporteMonetario { get; set; }

        [XmlIgnore]
        public bool ImporteMonetarioSpecified { get; set; }
    }
}
