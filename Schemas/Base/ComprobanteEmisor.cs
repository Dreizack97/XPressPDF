using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Base
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteEmisor
    {
        [XmlAttribute]
        public string Rfc { get; set; }

        [XmlAttribute]
        public string Nombre { get; set; }

        [XmlAttribute]
        public int RegimenFiscal { get; set; }

        [XmlAttribute]
        public string FacAtrAdquirente { get; set; }
    }
}