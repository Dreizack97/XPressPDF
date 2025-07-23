using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Base
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConceptoParte
    {
        [XmlElement("InformacionAduanera")]
        public ComprobanteConceptoParteInformacionAduanera[] InformacionAduanera { get; set; }

        [XmlAttribute]
        public string ClaveProdServ { get; set; }

        [XmlAttribute]
        public string NoIdentificacion { get; set; }

        [XmlAttribute]
        public decimal Cantidad { get; set; }

        [XmlAttribute]
        public string Unidad { get; set; }

        [XmlAttribute]
        public string Descripcion { get; set; }

        [XmlAttribute]
        public decimal ValorUnitario { get; set; }

        [XmlIgnore]
        public bool ValorUnitarioSpecified { get; set; }

        [XmlAttribute]
        public decimal Importe { get; set; }

        [XmlIgnore]
        public bool ImporteSpecified { get; set; }
    }
}
