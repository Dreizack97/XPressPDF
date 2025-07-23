using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Base
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteImpuestos
    {
        [XmlArrayItem("Retencion", IsNullable = false)]
        public ComprobanteImpuestosRetencion[] Retenciones { get; set; }

        [XmlArrayItem("Traslado", IsNullable = false)]
        public ComprobanteImpuestosTraslado[] Traslados { get; set; }

        [XmlAttribute]
        public decimal TotalImpuestosRetenidos { get; set; }

        [XmlIgnore]
        public bool TotalImpuestosRetenidosSpecified { get; set; }

        [XmlAttribute]
        public decimal TotalImpuestosTrasladados { get; set; }

        [XmlIgnore]
        public bool TotalImpuestosTrasladadosSpecified { get; set; }
    }
}
