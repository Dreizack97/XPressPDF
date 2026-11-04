namespace Core.Models.ConceptComplements.InstitucionesEducativas10;

using System;
using System.Xml.Serialization;
using Core.Abstractions;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/iedu")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/iedu", IsNullable = false)]
public sealed class InstEducativas : ICfdiConceptComplement
{
    [XmlIgnore]
    public string ComplementNamespace => "http://www.sat.gob.mx/iedu";

    [XmlIgnore]
    public string ConceptComplementName => "Instituciones Educativas Privadas 1.0";

    [XmlAttribute]
    public string version { get; set; } = "1.0";

    [XmlAttribute]
    public string nombreAlumno { get; set; } = string.Empty;

    [XmlAttribute]
    public string CURP { get; set; } = string.Empty;

    [XmlAttribute]
    public string nivelEducativo { get; set; } = string.Empty;

    [XmlAttribute]
    public string autRVOE { get; set; } = string.Empty;

    [XmlAttribute]
    public string? rfcPago { get; set; }
}
