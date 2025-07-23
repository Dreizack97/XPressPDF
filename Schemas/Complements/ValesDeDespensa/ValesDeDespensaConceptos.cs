using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.ValesDeDespensa
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/valesdedespensa")]
    public class ValesDeDespensaConceptos
    {
        [XmlElement("Concepto")]
        public ValesDeDespensaConceptosConcepto[] Concepto { get; set; }
    }
}
