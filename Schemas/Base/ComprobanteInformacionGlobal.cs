using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace XSD.Comprobante
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteInformacionGlobal
    {
        [XmlAttribute]
        public string Periodicidad { get; set; }

        [XmlAttribute]
        public string Meses { get; set; }

        [XmlAttribute]
        public short Año { get; set; }
    }
}
