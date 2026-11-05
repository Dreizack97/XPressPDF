namespace Core.Models.Complements.CartaPorte31;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteTiposFigura
{
    [XmlAttribute]
    public string TipoFigura { get; set; } = string.Empty;

    [XmlAttribute]
    public string RfcFigura { get; set; } = string.Empty;

    [XmlAttribute]
    public string? NumLicencia { get; set; }

    [XmlAttribute]
    public string? NombreFigura { get; set; }

    [XmlAttribute]
    public string? NumRegIdTribFigura { get; set; }

    [XmlAttribute]
    public string? ResidenciaFiscalFigura { get; set; }

    [XmlArrayItem("PartesTransporte", IsNullable = false)]
    public List<CartaPorteParteTransporte> PartesTransporte { get; set; } = [];

    [XmlElement]
    public CartaPorteDomicilio? Domicilio { get; set; }
}

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/CartaPorte31")]
public sealed class CartaPorteParteTransporte
{
    [XmlAttribute]
    public string ParteTransporte { get; set; } = string.Empty;
}
