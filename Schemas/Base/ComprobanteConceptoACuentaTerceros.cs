using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Base
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]

    public partial class ComprobanteConceptoACuentaTerceros
    {
        [XmlAttribute]
        public string RfcACuentaTerceros { get; set; } = string.Empty;

        [XmlAttribute]
        public string NombreACuentaTerceros { get; set; } = string.Empty;

        [XmlAttribute]
        public int RegimenFiscalACuentaTerceros { get; set; }

        [XmlAttribute]
        public string DomicilioFiscalACuentaTerceros { get; set; } = string.Empty;
    }
}
