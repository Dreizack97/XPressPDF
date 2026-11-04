namespace Core.Models.Complements.Divisas10;

using System;
using System.Xml.Serialization;
using Core.Abstractions;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/divisas")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/divisas", IsNullable = false)]
public sealed class Divisas : ICfdiComplement
{
    [XmlIgnore]
    public string ComplementNamespace => "http://www.sat.gob.mx/divisas";

    [XmlIgnore]
    public string ComplementName => "Divisas 1.0";

    [XmlAttribute]
    public string version { get; set; } = "1.0";

    [XmlAttribute]
    public string tipoOperacion { get; set; } = string.Empty;
}
