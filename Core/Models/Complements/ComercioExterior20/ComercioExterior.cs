namespace Core.Models.Complements.ComercioExterior20;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Core.Abstractions;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/ComercioExterior20")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/ComercioExterior20", IsNullable = false)]
public sealed class ComercioExterior : ICfdiComplement
{
    [XmlIgnore]
    public string ComplementNamespace => "http://www.sat.gob.mx/ComercioExterior20";

    [XmlIgnore]
    public string ComplementName => "Comercio Exterior 2.0";

    [XmlAttribute]
    public string Version { get; set; } = "2.0";

    [XmlAttribute]
    public string? MotivoTraslado { get; set; }

    [XmlAttribute]
    public string ClaveDePedimento { get; set; } = string.Empty;

    [XmlAttribute]
    public int CertificadoOrigen { get; set; }

    [XmlAttribute]
    public string? NumCertificadoOrigen { get; set; }

    [XmlAttribute]
    public string? NumeroExportadorConfiable { get; set; }

    [XmlAttribute]
    public string? Incoterm { get; set; }

    [XmlAttribute]
    public int Subdivisión { get; set; }

    [XmlIgnore]
    public bool SubdivisiónSpecified { get; set; }

    [XmlAttribute]
    public string? Observaciones { get; set; }

    [XmlAttribute]
    public decimal TipoCambioUSD { get; set; }

    [XmlIgnore]
    public bool TipoCambioUSDSpecified { get; set; }

    [XmlAttribute]
    public decimal TotalUSD { get; set; }

    [XmlIgnore]
    public bool TotalUSDSpecified { get; set; }

    [XmlArrayItem("Mercancia", IsNullable = false)]
    public List<ComercioExteriorMercancia> Mercancias { get; set; } = [];
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/ComercioExterior20")]
public sealed class ComercioExteriorMercancia
{
    [XmlAttribute]
    public string NoIdentificacion { get; set; } = string.Empty;

    [XmlAttribute]
    public string? FraccionArancelaria { get; set; }

    [XmlAttribute]
    public decimal CantidadAduana { get; set; }

    [XmlIgnore]
    public bool CantidadAduanaSpecified { get; set; }

    [XmlAttribute]
    public string? UnidadAduana { get; set; }

    [XmlAttribute]
    public decimal ValorUnitarioAduana { get; set; }

    [XmlIgnore]
    public bool ValorUnitarioAduanaSpecified { get; set; }

    [XmlAttribute]
    public decimal ValorDolares { get; set; }
}
