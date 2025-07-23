using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Base
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteCfdiRelacionados
    {
        [XmlElement("CfdiRelacionado")]
        public ComprobanteCfdiRelacionadosCfdiRelacionado[] CfdiRelacionado { get; set; } = Array.Empty<ComprobanteCfdiRelacionadosCfdiRelacionado>();

        [XmlAttribute]
        public string TipoRelacion { get; set; }
    }
}
