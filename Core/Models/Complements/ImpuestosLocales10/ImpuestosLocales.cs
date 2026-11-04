namespace Core.Models.Complements.ImpuestosLocales10;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Core.Abstractions;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/implocal")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/implocal", IsNullable = false)]
public sealed class ImpuestosLocales : ICfdiComplement
{
    [XmlIgnore]
    public string ComplementNamespace => "http://www.sat.gob.mx/implocal";

    [XmlIgnore]
    public string ComplementName => "Impuestos Locales 1.0";

    [XmlAttribute]
    public string version { get; set; } = "1.0";

    [XmlAttribute]
    public decimal TotaldeRetenciones { get; set; }

    [XmlAttribute]
    public decimal TotaldeTraslados { get; set; }

    [XmlElement("RetencionesLocales")]
    public List<ImpuestosLocalesRetencionLocal> RetencionesLocales { get; set; } = [];

    [XmlElement("TrasladosLocales")]
    public List<ImpuestosLocalesTrasladoLocal> TrasladosLocales { get; set; } = [];
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/implocal")]
public sealed class ImpuestosLocalesRetencionLocal
{
    [XmlAttribute]
    public string ImpLocRetenido { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal TasadeRetencion { get; set; }

    [XmlAttribute]
    public decimal Importe { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/implocal")]
public sealed class ImpuestosLocalesTrasladoLocal
{
    [XmlAttribute]
    public string ImpLocTrasladado { get; set; } = string.Empty;

    [XmlAttribute]
    public decimal TasadeTraslado { get; set; }

    [XmlAttribute]
    public decimal Importe { get; set; }
}
