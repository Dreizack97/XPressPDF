using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace XSD.Comprobante
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
    public partial class ComprobanteConcepto
    {
        public ComprobanteConceptoImpuestos Impuestos { get; set; }

        public ComprobanteConceptoACuentaTerceros ACuentaTerceros { get; set; }

        [XmlElement("InformacionAduanera")]
        public ComprobanteConceptoInformacionAduanera[] InformacionAduanera { get; set; } = Array.Empty<ComprobanteConceptoInformacionAduanera>();

        [XmlElement("CuentaPredial")]
        public ComprobanteConceptoCuentaPredial[] CuentaPredial { get; set; } = Array.Empty<ComprobanteConceptoCuentaPredial>();

        public ComprobanteConceptoComplementoConcepto ComplementoConcepto { get; set; }

        [XmlElement("Parte")]
        public ComprobanteConceptoParte[] Parte { get; set; } = Array.Empty<ComprobanteConceptoParte>();

        [XmlAttribute]
        public string ClaveProdServ { get; set; }

        [XmlAttribute]
        public string NoIdentificacion { get; set; } = string.Empty;

        [XmlAttribute]
        public decimal Cantidad { get; set; }

        [XmlAttribute]
        public string ClaveUnidad { get; set; }

        [XmlAttribute]
        public string Unidad { get; set; } = string.Empty;

        [XmlAttribute]
        public string Descripcion { get; set; } = string.Empty;

        [XmlAttribute]
        public decimal ValorUnitario { get; set; }

        [XmlAttribute]
        public decimal Importe { get; set; }

        [XmlAttribute]
        public decimal Descuento { get; set; }

        [XmlIgnore]
        public bool DescuentoSpecified { get; set; }

        [XmlAttribute]
        public string ObjetoImp { get; set; }
    }
}
