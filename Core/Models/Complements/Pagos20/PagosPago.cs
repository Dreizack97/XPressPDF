namespace Core.Models.Complements.Pagos20;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPago
{
    [XmlAttribute]
    public DateTime FechaPago { get; set; }

    [XmlAttribute]
    public string FormaDePagoP { get; set; } = string.Empty;

    [XmlAttribute]
    public string MonedaP { get; set; } = "MXN";

    [XmlAttribute]
    public decimal TipoCambioP { get; set; }

    [XmlIgnore]
    public bool TipoCambioPSpecified { get; set; }

    [XmlAttribute]
    public decimal Monto { get; set; }

    [XmlAttribute]
    public string? NumOperacion { get; set; }

    [XmlAttribute]
    public string? RfcEmisorCtaOrd { get; set; }

    [XmlAttribute]
    public string? NomBancoOrdExt { get; set; }

    [XmlAttribute]
    public string? CtaOrdenante { get; set; }

    [XmlAttribute]
    public string? RfcEmisorCtaBen { get; set; }

    [XmlAttribute]
    public string? CtaBeneficiario { get; set; }

    [XmlAttribute]
    public string? TipoCadPago { get; set; }

    [XmlAttribute]
    public string? CertPago { get; set; }

    [XmlAttribute]
    public string? CadPago { get; set; }

    [XmlAttribute]
    public string? SelloPago { get; set; }

    [XmlElement("DoctoRelacionado")]
    public List<PagosPagoDoctoRelacionado> DoctoRelacionado { get; set; } = [];

    [XmlElement]
    public PagosPagoImpuestosP? ImpuestosP { get; set; }
}
