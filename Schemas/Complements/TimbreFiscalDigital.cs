using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
    [XmlRoot(Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital", IsNullable = false)]
    public partial class TimbreFiscalDigital
    {
        [XmlAttribute]
        public string Version { get; set; } = "1.1";

        [XmlAttribute]
        public string UUID { get; set; } = string.Empty;

        [XmlAttribute]
        public DateTime FechaTimbrado { get; set; }

        [XmlAttribute]
        public string RfcProvCertif { get; set; } = string.Empty;

        [XmlAttribute]
        public string Leyenda { get; set; } = string.Empty;

        [XmlAttribute]
        public string SelloCFD { get; set; } = string.Empty;

        [XmlAttribute]
        public string NoCertificadoSAT { get; set; } = string.Empty;

        [XmlAttribute]
        public string SelloSAT { get; set; } = string.Empty;

        public string ComplementName => "TimbreFiscalDigital";
    }
}
