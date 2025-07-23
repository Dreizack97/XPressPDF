using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.ValesDeDespensa
{
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/valesdedespensa")]
    [XmlRoot(Namespace = "http://www.sat.gob.mx/valesdedespensa", IsNullable = false)]
    public partial class ValesDeDespensaConceptosConcepto
    {
        [XmlAttribute]
        public string identificador { get; set; }

        [XmlAttribute]
        public DateTime fecha { get; set; }

        [XmlAttribute]
        public string rfc { get; set; }

        [XmlAttribute]
        public string curp { get; set; }

        [XmlAttribute]
        public string nombre { get; set; }

        [XmlAttribute]
        public string numSeguridad { get; set; }

        [XmlAttribute]
        public decimal importe { get; set; }
    }
}