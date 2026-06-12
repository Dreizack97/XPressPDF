using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.ValesDeDespensa
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/valesdedespensa")]
    [XmlRoot(Namespace = "http://www.sat.gob.mx/valesdedespensa", IsNullable = false)]
    public partial class ValesDeDespensa
    {
        [XmlAttribute("version")]
        public string Version { get; set; } = "1.0";

        [XmlAttribute("tipoOperacion")]
        public string TipoOperacion { get; set; } = "monedero electrónico";

        [XmlAttribute("registroPatronal")]
        public string RegistroPatronal { get; set; }

        [XmlAttribute("numeroDeCuenta")]
        public string NumeroDeCuenta { get; set; }

        [XmlAttribute("total")]
        public decimal Total { get; set; }

        public ValesDeDespensaConceptos Conceptos { get; set; }
    }
}