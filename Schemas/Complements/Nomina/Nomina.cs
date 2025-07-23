using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Schemas.Complements.Nomina
{
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/nomina12")]
    [XmlRoot(Namespace = "http://www.sat.gob.mx/nomina12", IsNullable = false)]
    public partial class Nomina
    {
        public NominaEmisor Emisor { get; set; }

        public NominaReceptor Receptor { get; set; }

        public NominaPercepciones Percepciones { get; set; }

        public NominaDeducciones Deducciones { get; set; }

        [XmlArrayItem("OtroPago", IsNullable = false)]
        public NominaOtroPago[] OtrosPagos { get; set; }

        [XmlArrayItem("Incapacidad", IsNullable = false)]
        public NominaIncapacidad[] Incapacidades { get; set; }

        [XmlAttribute]
        public string Version { get; set; } = "1.2";

        [XmlAttribute]
        public string TipoNomina { get; set; }

        [XmlAttribute(DataType = "date")]
        public DateTime FechaPago { get; set; }

        [XmlAttribute(DataType = "date")]
        public DateTime FechaInicialPago { get; set; }

        [XmlAttribute(DataType = "date")]
        public DateTime FechaFinalPago { get; set; }

        [XmlAttribute]
        public decimal NumDiasPagados { get; set; }

        [XmlAttribute]
        public decimal TotalPercepciones { get; set; }

        [XmlIgnore]
        public bool TotalPercepcionesSpecified { get; set; }

        [XmlAttribute]
        public decimal TotalDeducciones { get; set; }

        [XmlIgnore]
        public bool TotalDeduccionesSpecified { get; set; }

        [XmlAttribute]
        public decimal TotalOtrosPagos { get; set; }

        [XmlIgnore]
        public bool TotalOtrosPagosSpecified { get; set; }
    }
}
