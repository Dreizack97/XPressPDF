namespace Core.Models.Complements.Pagos20;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPagoDoctoRelacionado
{
    [XmlAttribute]
    public string IdDocumento { get; set; } = string.Empty;

    [XmlAttribute]
    public string? Serie { get; set; }

    [XmlAttribute]
    public string? Folio { get; set; }

    [XmlAttribute]
    public string MonedaDR { get; set; } = "MXN";

    [XmlAttribute]
    public decimal EquivalenciaDR { get; set; }

    [XmlIgnore]
    public bool EquivalenciaDRSpecified { get; set; }

    [XmlAttribute]
    public string NumParcialidad { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal ImpSaldoAnt { get; set; }

    [XmlAttribute]
    public decimal ImpPagado { get; set; }

    [XmlAttribute]
    public decimal ImpSaldoInsoluto { get; set; }

    [XmlAttribute]
    public string ObjetoImpDR { get; set; } = string.Empty;

    [XmlElement]
    public PagosPagoDoctoRelacionadoImpuestosDR? ImpuestosDR { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPagoDoctoRelacionadoImpuestosDR
{
    [XmlArrayItem("RetencionDR", IsNullable = false)]
    public List<PagosPagoDoctoRelacionadoImpuestosDRRetencionDR> RetencionesDR { get; set; } = [];

    [XmlArrayItem("TrasladoDR", IsNullable = false)]
    public List<PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR> TrasladosDR { get; set; } = [];
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPagoDoctoRelacionadoImpuestosDRRetencionDR
{
    [XmlAttribute]
    public decimal BaseDR { get; set; }

    [XmlAttribute]
    public string ImpuestoDR { get; set; } = string.Empty;

    [XmlAttribute]
    public string TipoFactorDR { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal TasaOCuotaDR { get; set; }

    [XmlAttribute]
    public decimal ImporteDR { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
{
    [XmlAttribute]
    public decimal BaseDR { get; set; }

    [XmlAttribute]
    public string ImpuestoDR { get; set; } = string.Empty;

    [XmlAttribute]
    public string TipoFactorDR { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal TasaOCuotaDR { get; set; }

    [XmlIgnore]
    public bool TasaOCuotaDRSpecified { get; set; }

    [XmlAttribute]
    public decimal ImporteDR { get; set; }

    [XmlIgnore]
    public bool ImporteDRSpecified { get; set; }
}
