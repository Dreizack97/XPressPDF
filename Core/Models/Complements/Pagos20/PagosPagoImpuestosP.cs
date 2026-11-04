namespace Core.Models.Complements.Pagos20;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPagoImpuestosP
{
    [XmlArrayItem("RetencionP", IsNullable = false)]
    public List<PagosPagoImpuestosPRetencionP> RetencionesP { get; set; } = [];

    [XmlArrayItem("TrasladoP", IsNullable = false)]
    public List<PagosPagoImpuestosPTrasladoP> TrasladosP { get; set; } = [];
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPagoImpuestosPRetencionP
{
    [XmlAttribute]
    public string ImpuestoP { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal ImporteP { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public sealed class PagosPagoImpuestosPTrasladoP
{
    [XmlAttribute]
    public decimal BaseP { get; set; }

    [XmlAttribute]
    public string ImpuestoP { get; set; } = string.Empty;

    [XmlAttribute]
    public string TipoFactorP { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal TasaOCuotaP { get; set; }

    [XmlIgnore]
    public bool TasaOCuotaPSpecified { get; set; }

    [XmlAttribute]
    public decimal ImporteP { get; set; }

    [XmlIgnore]
    public bool ImportePSpecified { get; set; }
}
