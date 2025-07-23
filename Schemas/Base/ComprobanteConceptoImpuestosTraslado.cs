using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Base
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoImpuestosTraslado
    {
        [XmlAttribute]
        public decimal Base { get; set; }

        [XmlAttribute]
        public string Impuesto { get; set; }

        [XmlAttribute]
        public string TipoFactor { get; set; }

        [XmlAttribute]
        public decimal TasaOCuota { get; set; }

        [XmlIgnore]
        public bool TasaOCuotaSpecified { get; set; }

        [XmlAttribute]
        public decimal Importe { get; set; }

        [XmlIgnore]
        public bool ImporteSpecified { get; set; }
    }
}
