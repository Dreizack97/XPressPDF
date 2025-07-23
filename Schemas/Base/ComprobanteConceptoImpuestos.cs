using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Base
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoImpuestos
    {
        [XmlArrayItem("Traslado", IsNullable = false)]
        public ComprobanteConceptoImpuestosTraslado[] Traslados { get; set; } = Array.Empty<ComprobanteConceptoImpuestosTraslado>();

        [XmlArrayItem("Retencion", IsNullable = false)]
        public ComprobanteConceptoImpuestosRetencion[] Retenciones { get; set; } = Array.Empty<ComprobanteConceptoImpuestosRetencion>();
    }
}
