namespace Core.Models.Complements.Pagos20;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Core.Abstractions;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/Pagos20", IsNullable = false)]
public sealed class Pagos : ICfdiComplement
{
    [XmlIgnore]
    public string ComplementNamespace => "http://www.sat.gob.mx/Pagos20";

    [XmlIgnore]
    public string ComplementName => "Pagos 2.0";

    [XmlAttribute]
    public string Version { get; set; } = "2.0";

    [XmlElement]
    public PagosTotales? Totales { get; set; }

    [XmlElement("Pago")]
    public List<PagosPago> Pago { get; set; } = [];
}
