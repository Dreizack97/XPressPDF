namespace Core.Models.Complements.LeyendasFiscales10;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Core.Abstractions;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/leyendasFiscales")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/leyendasFiscales", IsNullable = false)]
public sealed class LeyendasFiscales : ICfdiComplement
{
    [XmlIgnore]
    public string ComplementNamespace => "http://www.sat.gob.mx/leyendasFiscales";

    [XmlIgnore]
    public string ComplementName => "Leyendas Fiscales 1.0";

    [XmlAttribute]
    public string version { get; set; } = "1.0";

    [XmlElement("Leyenda")]
    public List<LeyendasFiscalesLeyenda> Leyenda { get; set; } = [];
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/leyendasFiscales")]
public sealed class LeyendasFiscalesLeyenda
{
    [XmlAttribute]
    public string? disposicionFiscal { get; set; }

    [XmlAttribute]
    public string? norma { get; set; }

    [XmlAttribute]
    public string textoLeyenda { get; set; } = string.Empty;
}
